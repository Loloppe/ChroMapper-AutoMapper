using Beatmap.Base;
using Beatmap.V2;
using System.Collections.Generic;
using System.Linq;
using static Automapper.Items.Enumerator;
using static Automapper.Items.Utils;

namespace Automapper.Items
{
    internal class Helper
    {
        static public (BaseNote, BaseNote) FixDoublePlacement(BaseNote red, BaseNote blue)
        {
            int choice;
            int max;
            switch(red.CutDirection)
            {
                case CutDirection.UP:
                    switch(blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 13;
                            choice = RandNumber(0, max);
                            switch(choice)
                            {
                                case 0:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 4:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 5:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 6:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 7:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 8:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 9:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 10:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 11:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 12:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 9;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 4:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 5:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 6:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 7:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 8:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 8;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 2:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 3:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 5:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 6:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 7:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 7;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 5:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 6:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 3;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 1;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case CutDirection.DOWN:
                    switch (blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 9;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 4:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 5:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 6:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 7:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 8:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 6;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 4:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 5:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch(choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 6;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 1:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 2:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 4:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 5:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 4;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 3:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 1;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case CutDirection.LEFT:
                    switch (blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 7;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 5:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 6:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 6;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 5:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 5;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 9;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 5:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 6:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 7:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 8:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 6;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 5:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case CutDirection.RIGHT:
                    switch (blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 4;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 3;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 3:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 3;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 6;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 1;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 3;
                                    red.PosY = 1;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                                case 3:
                                    red.PosX = 3;
                                    red.PosY = 1;
                                    blue.PosX = 0;
                                    blue.PosY = 2;
                                    break;
                                case 4:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                                case 5:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 4;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 3:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 3;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case CutDirection.UP_LEFT:
                    switch (blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 7;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 4:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 5:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 6:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 4;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 3;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 4;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 3:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case CutDirection.UP_RIGHT:
                    switch (blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 3;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 2;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case CutDirection.DOWN_LEFT:
                    switch (blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 3;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 2;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 3;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 2:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 0;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case CutDirection.DOWN_RIGHT:
                    switch (blue.CutDirection)
                    {
                        case CutDirection.UP:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.LEFT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                        case CutDirection.RIGHT:
                            max = 3;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                                case 1:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 2:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_LEFT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.UP_RIGHT:
                            max = 2;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 2;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_LEFT:
                            max = 4;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 0;
                                    break;
                                case 1:
                                    red.PosX = 3;
                                    red.PosY = 0;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                                case 2:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 0;
                                    blue.PosY = 1;
                                    break;
                                case 3:
                                    red.PosX = 3;
                                    red.PosY = 1;
                                    blue.PosX = 1;
                                    blue.PosY = 0;
                                    break;
                            }
                            break;
                        case CutDirection.DOWN_RIGHT:
                            max = 1;
                            choice = RandNumber(0, max);
                            switch (choice)
                            {
                                case 0:
                                    red.PosX = 2;
                                    red.PosY = 0;
                                    blue.PosX = 3;
                                    blue.PosY = 1;
                                    break;
                            }
                            break;
                    }
                    break;
            }

            return (red, blue);
        }

        /// <summary>
        /// Verify if the note placement match the situation and return the value
        /// </summary>
        /// <param name="lastLine">Last note line</param>
        /// <param name="lastLayer">Last note layer</param>
        /// <param name="type">Note color</param>
        /// <returns>Line, Layer</returns>
        static public (int, int) PlacementCheck(int direction, int type)
        {
            // Next possible line and layer
            int line = -1;
            int layer = -1;
            int rand;

            if (type == ColorType.RED)
            {
                rand = RandNumber(0, PossibleRedPlacement.placement[direction].Count());
                line = PossibleRedPlacement.placement[direction][rand][0];
                layer = PossibleRedPlacement.placement[direction][rand][1];
            }
            else if (type == ColorType.BLUE)
            {
                rand = RandNumber(0, PossibleBluePlacement.placement[direction].Count());
                line = PossibleBluePlacement.placement[direction][rand][0];
                layer = PossibleBluePlacement.placement[direction][rand][1];
            }

            return (line, layer);
        }

        /// <summary>
        /// Verify if the next direction selected match the situation (flow free) and return the value
        /// </summary>
        /// <param name="last">Last direction</param>
        /// <param name="swing">Up or down swing (wrist)</param>
        /// <param name="hand">Current hand</param>
        /// <param name="speed"></param>
        /// <returns>Direction</returns>
        static public int NextDirection(int last, int swing, int hand, float speed, bool limiter)
        {
            // Store all the possible next cut direction, we will use some logic to find if the next direction match
            int[] possibleNext = { 0, 0 };
            // Type of flow
            int flow = 0;
            // Next direction
            int next;

            if(Options.Mapper.UpDownOnly)
            {
                if(swing == 0)
                {
                    return 1;
                }
                else if (swing == 1)
                {
                    return 0;
                }
            }

            // Get the direction based on speed
            if (hand == 0)
            {
                if (speed < 0.5) // Under half a beat
                {
                    possibleNext = PossibleFlow.normalRed[last];
                    flow = 2;
                }
                else if (speed >= 0.5 && speed < 1) // Half to under a beat
                {
                    possibleNext = PossibleFlow.techRed[last];
                    flow = 1;
                }
                else // Anything above a beat is pretty slow usually
                {
                    possibleNext = PossibleFlow.extremeRed[last];
                    flow = 0;
                }
            }
            else if (hand == 1)
            {
                if (speed < 0.5) // Under half a beat
                {
                    possibleNext = PossibleFlow.normalBlue[last];
                    flow = 2;
                }
                else if (speed >= 0.5 && speed < 1) // Half to under a beat
                {
                    possibleNext = PossibleFlow.techBlue[last];
                    flow = 1;
                }
                else // Anything above a beat is pretty slow usually
                {
                    possibleNext = PossibleFlow.extremeBlue[last];
                    flow = 0;
                }
            }


            do
            {
                next = Utils.RandNumber(0, 8);

                if (hand == 0)
                {
                    if (PossibleFlow.extremeRed[last].Contains(next) && !PossibleFlow.techRed[last].Contains(next)) // Extreme roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                    if ((PossibleFlow.extremeRed[last].Contains(next) || PossibleFlow.techRed[last].Contains(next)) && !PossibleFlow.normalRed[last].Contains(next)) // Extreme and tech roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                }
                else if (hand == 1)
                {
                    if (PossibleFlow.extremeBlue[last].Contains(next) && !PossibleFlow.techBlue[last].Contains(next)) // Extreme roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                    if ((PossibleFlow.extremeBlue[last].Contains(next) || PossibleFlow.techBlue[last].Contains(next)) && !PossibleFlow.normalBlue[last].Contains(next)) // Extreme and tech roll again
                    {
                        next = Utils.RandNumber(0, 8);
                    }
                }




                // We check if the possible next direction match with the last one before any logic.
                if (possibleNext.Contains(next))
                {
                    // Each hand and type of swing have to be treated differently
                    if (hand == 0) // Red
                    {
                        if (swing == 0) // Up Swing
                        {
                            if (limiter && (next == CutDirection.LEFT || next == CutDirection.UP_RIGHT || next == CutDirection.UP)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.LEFT)
                            {
                                if (next == CutDirection.DOWN && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.DOWN || last == CutDirection.DOWN_LEFT) // Down, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.LEFT || next == CutDirection.UP_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            else if (last == CutDirection.RIGHT || last == CutDirection.UP_RIGHT) // Right, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.UP || next == CutDirection.UP_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.UP)
                            {
                                if ((next == CutDirection.RIGHT) && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.DOWN_LEFT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                        else if (swing == 1) // Down Swing
                        {
                            if (limiter && (next == CutDirection.RIGHT || next == CutDirection.UP_RIGHT || next == CutDirection.DOWN || next == CutDirection.DOWN_LEFT)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.RIGHT) //Meh
                            {
                                if (next == CutDirection.UP && flow != 0)
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.UP || last == CutDirection.UP_RIGHT) // Up, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.RIGHT || next == CutDirection.DOWN_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.LEFT || last == CutDirection.DOWN_LEFT) // Left, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.DOWN || next == CutDirection.DOWN_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.DOWN)
                            {
                                if ((next == CutDirection.RIGHT || next == CutDirection.UP_RIGHT) && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.LEFT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    else if (hand == 1) // Blue
                    {
                        if (swing == 0) // Up Swing
                        {
                            if (limiter && (next == CutDirection.RIGHT || next == CutDirection.UP || next == CutDirection.UP_LEFT)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.RIGHT)
                            {
                                if (next == CutDirection.DOWN && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.DOWN || last == CutDirection.DOWN_RIGHT) // Down, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.RIGHT || next == CutDirection.UP_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.LEFT || last == CutDirection.UP_LEFT) // Left, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.UP || next == CutDirection.UP_RIGHT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.UP)
                            {
                                if (next == CutDirection.LEFT && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.DOWN_RIGHT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                        else if (swing == 1) // Down Swing
                        {
                            if (limiter && (next == CutDirection.LEFT || next == CutDirection.UP_LEFT || next == CutDirection.DOWN || next == CutDirection.DOWN_RIGHT)) // Too far
                            {
                                continue;
                            }

                            if (last == CutDirection.LEFT) //Meh
                            {
                                if (next == CutDirection.UP && flow != 0)
                                {
                                    continue;
                                }
                            }

                            if (last == CutDirection.UP || last == CutDirection.UP_LEFT) // Up, maximum range of the wrist from the right (extreme)
                            {
                                if (next == CutDirection.LEFT || next == CutDirection.DOWN_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.RIGHT || last == CutDirection.DOWN_RIGHT) // Right, maximum range of the wrist from the left (extreme)
                            {
                                if (next == CutDirection.DOWN || next == CutDirection.DOWN_LEFT) // Impossible
                                {
                                    continue;
                                }
                            }
                            if (last == CutDirection.DOWN)
                            {
                                if ((next == CutDirection.LEFT || next == CutDirection.UP_LEFT) && flow != 0) // Not extreme
                                {
                                    continue;
                                }
                                if (next == CutDirection.RIGHT && flow != 0) // Meh
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    return next;
                }
            } while (true);
        }

        /// <summary>
        /// Method to find specific pattern and remove the notes of the pattern from the main list of note
        /// </summary>
        /// <param name="notes">List of ColorNote</param>
        /// <returns>List of list of ColorNote (Pattern) and modified List of ColorNote</returns>
        static public (List<List<BaseNote>>, List<BaseNote>) FindPattern(List<BaseNote> notes)
        {
            // List of list to keep thing like sliders/stack/window/tower etc
            List<List<BaseNote>> patterns = new List<List<BaseNote>>();

            // Stock pattern notes
            List<BaseNote> pattern = new List<BaseNote>();

            // To know if a pattern was found
            bool found = false;

            // Find all notes sliders/stack/window/tower
            for (int i = 0; i < notes.Count; i++)
            {
                if (i == notes.Count - 1)
                {
                    if (found)
                    {
                        BaseNote n = new V2Note
                        {
                            Time = notes[i].Time,
                            PosX = notes[i].PosX,
                            PosY = notes[i].PosY,
                            Type = notes[i].Type,
                            CutDirection = notes[i].CutDirection
                        };
                        pattern.Add(n);
                        notes.RemoveAt(i);
                        patterns.Add(new List<BaseNote>(pattern));
                    }
                    break;
                }

                BaseNote now = notes[i];
                BaseNote next = notes[i + 1];

                if (next.Time - now.Time >= 0 && next.Time - now.Time < 0.1)
                {
                    if (!found)
                    {
                        pattern = new List<BaseNote>();
                        found = true;
                    }
                    BaseNote n = new V2Note
                    {
                        Time = notes[i].Time,
                        PosX = notes[i].PosX,
                        PosY = notes[i].PosY,
                        Type = notes[i].Type,
                        CutDirection = notes[i].CutDirection
                    };
                    pattern.Add(n);
                    notes.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (found)
                    {
                        BaseNote n = new V2Note
                        {
                            Time = notes[i].Time,
                            PosX = notes[i].PosX,
                            PosY = notes[i].PosY,
                            Type = notes[i].Type,
                            CutDirection = notes[i].CutDirection
                        };
                        pattern.Add(n);
                        notes.RemoveAt(i);
                        i--;
                        patterns.Add(new List<BaseNote>(pattern));
                    }

                    found = false;
                }
            }

            return (patterns, notes);
        }
    }
}
