// MIT License

// Copyright (c) 2024 Loloppe

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
using UnityEngine;
using Beatmap.Base;
using System.Threading.Tasks;

namespace Automapper
{
    [Plugin("Automapper")]
    public class Automapper
    {
        private UI.UI _ui;
        static public BeatSaberSongContainer _beatSaberSongContainer;
        private NoteGridContainer _noteGridContainer;
        private ObstacleGridContainer _obstacleGridContainer;
        private EventGridContainer _eventGridContainer;

        [Init]
        private void Init()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            _ui = new UI.UI(this);
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex == 3)
            {
                _noteGridContainer = Object.FindObjectOfType<NoteGridContainer>();
                _eventGridContainer = Object.FindObjectOfType<EventGridContainer>();
                _obstacleGridContainer = Object.FindObjectOfType<ObstacleGridContainer>();
                _beatSaberSongContainer = Object.FindObjectOfType<BeatSaberSongContainer>();

                 MapEditorUI mapEditorUI = Object.FindObjectOfType<MapEditorUI>();
                _ui.AddMenu(mapEditorUI);
            }
        }

        public async Task LightAsync()
        {
            if(_noteGridContainer.MapObjects.Any())
            {
                List<BaseNote> notes = _noteGridContainer.MapObjects.ToList();
                notes = notes.OrderBy(o => o.JsonTime).ToList();

                List<BaseNote> select = null;

                var selection = SelectionController.SelectedObjects;
                if (selection.Count > 0)
                {
                    if (selection.All(x => x is BaseNote))
                    {
                        select = new List<BaseNote>(selection.Cast<BaseNote>());
                        if (notes.Exists(o => o.JsonTime > select.First().JsonTime && o.JsonTime < select.Last().JsonTime && !select.Contains(o)))
                        {
                            Debug.Log("Automapper: This is not a whole section selection. Notes might be missing.");
                            select = null;
                        }
                    }
                }
                else
                {
                    select = notes;
                }

                if(select != null)
                {
                    await Task.Run(() =>
                    {
                        select = select.OrderBy(o => o.JsonTime).ToList();

                        if (Options.Light.IgnoreBomb)
                        {
                            select = new List<BaseNote>(select.Where(x => x.Type != Enumerator.NoteType.BOMB));
                        }

                        // Get new events
                        List<BaseEvent> newEvents = Methods.Light.CreateLight(_noteGridContainer.MapObjects.ToList(), select);

                        // Back on main thread for Unity operations
                        UnityThreadDispatcher.Instance().Enqueue(() =>
                        {
                            List<BaseEvent> oldEvents = _eventGridContainer.MapObjects.Where(ev => Utils.EnvironmentEvent.IsEnvironmentEvent(ev) &&
                                ev.JsonTime >= select.First().JsonTime && ev.JsonTime <= select.Last().JsonTime).ToList();

                            // Delete old events
                            foreach (var ev in oldEvents)
                            {
                                _eventGridContainer.DeleteObject(ev, false);
                            }

                            // Add new events
                            foreach (var ev in newEvents)
                            {
                                _eventGridContainer.SpawnObject(ev, false, false, true);
                            }

                            _eventGridContainer.DoPostObjectsSpawnedWorkflow();
                            _eventGridContainer.RefreshPool(true);
                        });
                    });
                }
            }
        }

        public async Task ConverterAsync()
        {
            if (_noteGridContainer.MapObjects.Any())
            {
                List<BaseNote> notes = _noteGridContainer.MapObjects.Where(n => n.Type != 3).ToList();
                notes = notes.OrderBy(o => o.JsonTime).ToList();
                List<BaseNote> select = null;

                var selection = SelectionController.SelectedObjects;
                if (selection.Count > 0)
                {
                    if (selection.All(x => x is BaseNote))
                    {
                        select = new List<BaseNote>(selection.Cast<BaseNote>());
                        if (notes.Exists(o => o.JsonTime >= select.First().JsonTime && o.JsonTime <= select.Last().JsonTime && !select.Contains(o)))
                        {
                            Debug.LogWarning("Automapper: This is not a whole section selection. Make sure to take all notes in the range and double on same beat.");
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
                    await Task.Run(() =>
                    {
                        // Move existing conversion logic into the task
                        if (select.Contains(notes[1]) && !select.Contains(notes.First()))
                        {
                            select.Add(notes.First());
                            select = select.OrderBy(o => o.JsonTime).ToList();
                        }

                        List<BaseNote> redNotes = new List<BaseNote>();
                        List<BaseNote> blueNotes = new List<BaseNote>();
                        List<BaseNote> toDelete = new List<BaseNote>(select);
                        select = new List<BaseNote>();

                        if (select.Exists(o => o.Type == 0))
                        {
                            redNotes = new List<BaseNote>(select.Where(w => w.Type == 0).ToList());
                        }

                        if (select.Exists(o => o.Type == 1))
                        {
                            blueNotes = new List<BaseNote>(select.Where(w => w.Type == 1).ToList());
                        }

                        List<List<BaseNote>> patterns = new List<List<BaseNote>>();

                        if (redNotes.Any() && redNotes.Count > 1)
                        {
                            (patterns, redNotes) = Helper.FindPattern(redNotes);
                            foreach (List<BaseNote> pattern in patterns)
                            {
                                redNotes.Add(pattern[0]);
                            }
                            select.AddRange(redNotes);
                        }

                        if (blueNotes.Any() && blueNotes.Count > 1)
                        {
                            patterns = new List<List<BaseNote>>();
                            (patterns, blueNotes) = Helper.FindPattern(blueNotes);
                            foreach (List<BaseNote> pattern in patterns)
                            {
                                blueNotes.Add(pattern[0]);
                            }
                            select.AddRange(blueNotes);
                        }

                        // Process timings and generate new notes
                        List<float> timings = new List<float>();
                        BaseNote lastBlue = null;
                        BaseNote lastRed = null;

                        if (select.Any())
                        {
                            select = select.OrderBy(o => o.JsonTime).ToList();
                            foreach (BaseNote note in select)
                            {
                                timings.Add(note.JsonTime);
                            }

                            if (!select.Contains(notes.First()) && !select.Contains(notes[1]))
                            {
                                if(select.First().Type == 0)
                                {
                                    lastRed = notes.FindLast(o => o.JsonTime < select.First().JsonTime && o.Type == select.First().Type);
                                    lastBlue = notes.FindLast(o => o.JsonTime < select.First().JsonTime && o.Type != select.First().Type);
                                }
                                else
                                {
                                    lastBlue = notes.FindLast(o => o.JsonTime < select.First().JsonTime && o.Type == select.First().Type);
                                    lastRed = notes.FindLast(o => o.JsonTime < select.First().JsonTime && o.Type != select.First().Type);
                                }
                            }
                            else if (!select.Contains(notes.First()) && select.First().JsonTime != notes[0].JsonTime && notes.Count > 1)
                            {
                                int index = notes.FindIndex(o => o == select.First());
                                if (notes[index - 1].Type == 0)
                                {
                                    lastRed = notes[index - 1];
                                    lastBlue = new BaseNote { Type = 1 };
                                }
                                else
                                {
                                    lastBlue = notes[index - 1];
                                    lastRed = new BaseNote { Type = 0 };
                                }
                            }

                            List<BaseNote> newNotes = Methods.NoteGenerator.AutoMapper(timings, BeatSaberSongContainer.Instance.Info.BeatsPerMinute, select.First().Type, lastRed, lastBlue);

                            // Back on main thread for Unity operations
                            UnityThreadDispatcher.Instance().Enqueue(() =>
                            {
                                foreach (var n in toDelete)
                                {
                                    _noteGridContainer.DeleteObject(n, false);
                                }

                                List<BaseObstacle> obstacles = _obstacleGridContainer.MapObjects.ToList();
                                foreach (var o in obstacles)
                                {
                                    _obstacleGridContainer.DeleteObject(o, false, inCollectionOfDeletes: true);
                                }

                                foreach (var n in newNotes)
                                {
                                    _noteGridContainer.SpawnObject(n, false, false, true);
                                    selection.Add(n);
                                }

                                _obstacleGridContainer.DoPostObjectsDeleteWorkflow();
                                _noteGridContainer.DoPostObjectsSpawnedWorkflow();

                                _obstacleGridContainer.RefreshPool(true);
                                _noteGridContainer.RefreshPool(true);
                            });
                        }
                    });
                }
            }
        }

        public async Task AudioAsync()
        {
            await Task.Run(() =>
            {
                List<BaseNote> newNotes = Methods.Onset.GetMap("song.ogg", BeatSaberSongContainer.Instance.Info.BeatsPerMinute);

                // Back on main thread for Unity operations
                UnityThreadDispatcher.Instance().Enqueue(() =>
                {
                    List<BaseNote> notes = _noteGridContainer.MapObjects.ToList();
                    List<BaseObstacle> obstacles = _obstacleGridContainer.MapObjects.ToList();

                    // Delete old obstacles within range
                    foreach (var o in obstacles)
                    {
                        if (o.JsonTime >= Options.Mapper.MinRange && o.JsonTime <= Options.Mapper.MaxRange)
                        {
                            _obstacleGridContainer.DeleteObject(o, false, inCollectionOfDeletes: true);
                        }
                    }

                    // Delete old notes within range
                    foreach (var n in notes)
                    {
                        if(n.JsonTime >= Options.Mapper.MinRange && n.JsonTime <= Options.Mapper.MaxRange)
                        {
                            _noteGridContainer.DeleteObject(n, false, inCollectionOfDeletes: true);
                        }
                    }

                    // Add new notes within range
                    foreach (var n in newNotes)
                    {
                        if (n.JsonTime >= Options.Mapper.MinRange && n.JsonTime <= Options.Mapper.MaxRange)
                        {
                            _noteGridContainer.SpawnObject(n, false, false, true);
                        }
                    }

                    _obstacleGridContainer.DoPostObjectsDeleteWorkflow();
                    _noteGridContainer.DoPostObjectsSpawnedWorkflow();

                    _obstacleGridContainer.RefreshPool(true);
                    _noteGridContainer.RefreshPool(true);
                });
            });
        }

        // Keep the original synchronous methods as wrappers for backward compatibility in case someone uses them in a different mod i mean who knows but idk
        public void Light()
        {
            _ = LightAsync();
        }

        public void Converter()
        {
            _ = ConverterAsync();
        }

        public void Audio()
        {
            _ = AudioAsync();
        }

        [Exit]
        private void Exit() { }
    }
}
