// MIT License

// Copyright (c) 2022 Loloppe

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// Dependencies (MIT Licences):
// https://github.com/naudio/NAudio
// https://github.com/naudio/Vorbis

using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Automapper.Items;
using Automapper.UserInterface;
using UnityEngine;

namespace Automapper
{
    [Plugin("Automapper")]
    public class Automapper
    {
        private UI _ui;
        static public BeatSaberSongContainer _beatSaberSongContainer;
        private NotesContainer _notesContainer;
        private EventsContainer _eventsContainer;
        private ObstaclesContainer _obstaclesContainer;

        [Init]
        private void Init()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            _ui = new UI(this);
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex == 3)
            {
                _notesContainer = Object.FindObjectOfType<NotesContainer>();
                _eventsContainer = Object.FindObjectOfType<EventsContainer>();
                _obstaclesContainer = Object.FindObjectOfType<ObstaclesContainer>();
                _beatSaberSongContainer = Object.FindObjectOfType<BeatSaberSongContainer>();

                 MapEditorUI mapEditorUI = Object.FindObjectOfType<MapEditorUI>();
                _ui.AddMenu(mapEditorUI);
            }
        }

        public void Light()
        {
            if(_notesContainer.LoadedObjects.Count != 0)
            {
                List<BeatmapNote> notes = _notesContainer.LoadedObjects.Cast<BeatmapNote>().ToList();
                notes = notes.OrderBy(o => o.Time).ToList();
                List<MapEvent> oldEvents = _eventsContainer.LoadedObjects.Cast<MapEvent>().Where(ev => Utils.EnvironmentEvent.IsEnvironmentEvent(ev)).ToList();

                if (Options.Light.IgnoreBomb)
                {
                    notes = new List<BeatmapNote>(notes.Where(x => x.Type != Enumerator.NoteType.BOMB));
                }

                // Get new events
                List<MapEvent> newEvents = Methods.Light.CreateLight(notes);

                // Delete old events
                foreach (var ev in oldEvents)
                {
                    _eventsContainer.DeleteObject(ev, false);
                }

                // Add new events
                foreach (var ev in newEvents)
                {
                    _eventsContainer.SpawnObject(ev, false, false);
                }

                BeatmapObjectContainerCollection.GetCollectionForType(BeatmapObject.ObjectType.Event).RefreshPool(true);
            }
        }

        public void Converter()
        {
            if (_notesContainer.LoadedObjects.Count != 0)
            {
                List<BeatmapNote> notes = new List<BeatmapNote>(_notesContainer.LoadedObjects.Cast<BeatmapNote>().ToList());
                notes = notes.OrderBy(o => o.Time).ToList();
                List<BeatmapNote> select = null;

                var selection = SelectionController.SelectedObjects;
                if (selection.Count > 0)
                {
                    if (selection.All(x => x is BeatmapNote))
                    {
                        select = new List<BeatmapNote>(selection.Cast<BeatmapNote>());
                        if (notes.Exists(o => o.Time > select.First().Time && o.Time < select.Last().Time && !select.Contains(o)))
                        {
                            // This is not a whole selection
                            Debug.Log("Automapper: This is not a whole section selection. Notes might be missing.");
                            select = null;
                        }
                    }
                }
                else
                {
                    select = notes;
                }

                if (select != null)
                {
                    // Add the first note if the second note is selected
                    if (select.Contains(notes[1]) && !select.Contains(notes.First()))
                    {
                        select.Add(notes.First());
                        select.OrderBy(o => o.Time);
                    }

                    List<BeatmapNote> redNotes = new List<BeatmapNote>();
                    List<BeatmapNote> blueNotes = new List<BeatmapNote>();

                    // Separate note per type
                    if (select.Exists(o => o.Type == 0))
                    {
                        redNotes = new List<BeatmapNote>(select.Where(w => w.Type == 0).ToList());
                    }

                    if (select.Exists(o => o.Type == 1))
                    {
                        blueNotes = new List<BeatmapNote>(select.Where(w => w.Type == 1).ToList());
                    }

                    // We do nothing with patterns for now
                    List<List<BeatmapNote>> patterns = new List<List<BeatmapNote>>();
                    List<BeatmapNote> toDelete = new List<BeatmapNote>(select);
                    select = new List<BeatmapNote>();

                    if (redNotes.Any())
                    {
                        (patterns, redNotes) = Helper.FindPattern(redNotes);

                        // We keep the first note of each pattern
                        foreach (List<BeatmapNote> pattern in patterns)
                        {
                            redNotes.Add(pattern[0]);
                        }

                        select.AddRange(redNotes);
                    }

                    if (blueNotes.Any())
                    {
                        // We do nothing with patterns for now
                        patterns = new List<List<BeatmapNote>>();
                        (patterns, blueNotes) = Helper.FindPattern(blueNotes);

                        // We keep the first note of each pattern
                        foreach (List<BeatmapNote> pattern in patterns)
                        {
                            blueNotes.Add(pattern[0]);
                        }

                        select.AddRange(blueNotes);
                    }

                    // Delete old notes
                    foreach (var n in toDelete)
                    {
                        _notesContainer.DeleteObject(n, false);
                    }

                    List<float> timings = new List<float>();

                    if (select.Count > 0)
                    {
                        select = select.OrderBy(o => o.Time).ToList();
                        foreach (BeatmapNote note in select)
                        {
                            timings.Add(note.Time);
                        }

                        BeatmapNote before = null;
                        BeatmapNote beforeBefore = null;

                        if (!select.Contains(notes.First()) && !select.Contains(notes[1]) && select.First().Time != notes.First().Time && select[1].Time != notes[1].Time && notes.Count > 1)
                        {
                            int index = notes.FindIndex(o => o.Time == select.First().Time && o.Type == select.First().Type);
                            Debug.Log(index);
                            before = notes[index - 2];
                            beforeBefore = notes[index - 1];
                        }
                        else if (!select.Contains(notes.First()) && select.First().Time != notes[0].Time && notes.Count > 1)
                        {
                            int index = notes.FindIndex(o => o.Time == select.First().Time && o.Type == select.First().Type);
                            Debug.Log(index);
                            before = notes[index - 1];
                            beforeBefore = new BeatmapNote();
                            beforeBefore.Type = notes[index - 1].Type;
                            if (beforeBefore.Type == 0)
                            {
                                beforeBefore.Type = 1;
                            }
                            else
                            {
                                beforeBefore.Type = 0;
                            }
                        }

                        // Get new notes
                        List<BeatmapNote> no = Methods.NoteGenerator.AutoMapper(timings, BeatSaberSongContainer.Instance.Song.BeatsPerMinute, select.First().Type, before, beforeBefore);

                        List<BeatmapObstacle> obstacles = _obstaclesContainer.LoadedObjects.Cast<BeatmapObstacle>().ToList();

                        // Delete old obstacles
                        foreach (var o in obstacles)
                        {
                            _obstaclesContainer.DeleteObject(o, false);
                        }

                        // Add new notes
                        foreach (var n in no)
                        {
                            _notesContainer.SpawnObject(n, false, false);
                            selection.Add(n);
                        }
                    }

                    BeatmapObjectContainerCollection.GetCollectionForType(BeatmapObject.ObjectType.Obstacle).RefreshPool(true);
                    BeatmapObjectContainerCollection.GetCollectionForType(BeatmapObject.ObjectType.Note).RefreshPool(true);
                }
            }
        }

        public void Audio()
        {
            List<BeatmapNote> no;
            no = Methods.Onset.GetMap("song.ogg", BeatSaberSongContainer.Instance.Song.BeatsPerMinute);

            List<BeatmapNote> notes = _notesContainer.LoadedObjects.Cast<BeatmapNote>().ToList();
            List<BeatmapObstacle> obstacles = _obstaclesContainer.LoadedObjects.Cast<BeatmapObstacle>().ToList();

            // Delete old obstacles
            foreach (var o in obstacles)
            {
                _obstaclesContainer.DeleteObject(o, false);
            }

            // Delete old notes
            foreach (var n in notes)
            {
                _notesContainer.DeleteObject(n, false);
            }

            // Add new notes
            foreach (var n in no)
            {
                _notesContainer.SpawnObject(n, false, false);
            }

            BeatmapObjectContainerCollection.GetCollectionForType(BeatmapObject.ObjectType.Obstacle).RefreshPool(true);
            BeatmapObjectContainerCollection.GetCollectionForType(BeatmapObject.ObjectType.Note).RefreshPool(true);
        }

        [Exit]
        private void Exit() { }
    }
}