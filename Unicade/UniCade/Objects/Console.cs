using System;
using System.Collections.Generic;
using System.Linq;
using UniCade.Backend;
using UniCade.Interfaces;
using UniCade.Constants;

namespace UniCade.Objects
{
    internal class Console : IConsole
    {
        #region Properties

        /// <summary>
        /// The common display name for the console
        /// </summary>
        public string ConsoleName
        {
            get => _consoleName;
            set
            {
                if (_consoleName != null)
                {
                    if (value == null)
                    {
                        throw new ArgumentException("Console name cannot be null");
                    }
                    if (value.Length == 0)
                    {
                        throw new ArgumentException("Console name cannot be empty");
                    }
                    if (Utilties.CheckForInvalidChars(value))
                    {
                        throw new ArgumentException("Console name contains invalid characters");
                    }
                    if (value.Length > ConstValues.MAX_CONSOLE_NAME_LENGTH)
                    {
                        throw new ArgumentException(String.Format("Console name cannot exceed {0} chars",
                            ConstValues.MAX_CONSOLE_NAME_LENGTH));
                    }
                }
                _consoleName = value;
            }
        }

        /// <summary>
        /// The original release date for the console
        /// </summary>
        public string ReleaseDate
        {
            get => _releaseDate;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Release date cannot be null");
                }
                if (!Utilties.IsAllDigits(value))
                {
                    throw new ArgumentException("Release date must be only digits");
                }
                if (value.Length != 4)
                {
                    throw new ArgumentException("Release date must be four digits");
                }
                _releaseDate = value;
            }
        }

        /// <summary>
        /// Full path for the emulators folder
        /// </summary>
        public string EmulatorPath
        {
            get => _emulatorPath;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Emulator path cannot be null");
                }
                if (value.Length < 4)
                {
                    throw new ArgumentException("Emulator path too short");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Emulator path contains invalid characters");
                }
                if (!value.Contains(":\\"))
                {
                    throw new ArgumentException("Emulator path invalid");
                }
                if (value.Length > ConstValues.MAX_PATH_LENGTH)
                {
                    throw new ArgumentException(String.Format("Emulator path cannot exceed {0} chars", ConstValues.MAX_PATH_LENGTH));
                }
                _emulatorPath = value;
            }
        }

        /// <summary>
        /// The full path for the console preferences file
        /// </summary>
        public string PreferencesPath
        {
            get => _preferencesPath;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Preferences path cannot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Preferences path contains invalid characters");
                }
                if (value.Length < 4)
                {
                    throw new ArgumentException("Preferences path too short");
                }
                if (value.Length > ConstValues.MAX_PATH_LENGTH)
                {
                    throw new ArgumentException(String.Format("Preferences path cannot exceed {0} chars", ConstValues.MAX_PATH_LENGTH));
                }
                _preferencesPath = value;
            }
        }

        /// <summary>
        /// The full path to the rom directory for the current console
        /// </summary>
        public string RomPath
        {
            get => _romPath;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("ROM path cannot be null");
                }
                if (value.Length < 4)
                {
                    throw new ArgumentException("ROM path too short");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("ROM path contains invalid characters");
                }
                if (!value.Contains(":\\"))
                {
                    throw new ArgumentException("ROM path invalid");
                }
                if (value.Length > ConstValues.MAX_PATH_LENGTH)
                {
                    throw new ArgumentException(String.Format("ROM path cannot exceed {0} chars", ConstValues.MAX_PATH_LENGTH));
                }
                _romPath = value;
            }
        }

        /// <summary>
        /// The extensions for the current console
        /// </summary>
        public string RomExtension
        {
            get => _romExtensions;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("ROM file extension cannnot be null");
                }
                if (value.Length == 0)
                {
                    throw new ArgumentException("ROM file extension cannnot be empty");
                }
                if (Utilties.CheckForInvalidSplitChars(value))
                {
                    throw new ArgumentException("File extension contains invalid characters");
                }
                if (!value.Contains("."))
                {
                    throw new ArgumentException("File extension invalid");
                }
                if (value.Length > ConstValues.MAX_FILE_EXT_LENGTH)
                {
                    throw new ArgumentException(String.Format("ROM extension length cannot exceed {0} chars", ConstValues.MAX_FILE_EXT_LENGTH));
                }
                _romExtensions = value;
            }
        }

        /// <summary>
        /// Basic console description and info
        /// </summary>
        public string ConsoleInfo
        {
            get => _consoleInfo;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Console info cannnot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Console info contains invalid characters");
                }
                if (value.Length > ConstValues.MAX_CONSOLE_INFO_LENGTH)
                {
                    throw new ArgumentException(String.Format("ROM extension length cannot exceed {0} chars", ConstValues.MAX_CONSOLE_INFO_LENGTH));
                }
                _consoleInfo = value;
            }
        }

        /// <summary>
        /// The launch params for the current emulator
        /// </summary>
        public string LaunchParams
        {
            get => _launchParams;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Launch params cannnot be null");
                }
                if (Utilties.CheckForInvalidChars(value))
                {
                    throw new ArgumentException("Launch params contain invalid characters");
                }
                if (value.Length > ConstValues.MAX_LAUNCH_PARAMS_LENGTH)
                {
                    throw new ArgumentException(
                        $"Launch params length cannot exceed {ConstValues.MAX_LAUNCH_PARAMS_LENGTH} chars");
                }
                _launchParams = value;
            }
        }

        #endregion

        #region  Private Instance Fields

        /// <summary>
        /// The common display name for the console
        /// </summary>
        private string _consoleName;

        /// <summary>
        /// The original release date for the console
        /// </summary>
        private string _releaseDate;

        /// <summary>
        /// Full path for the emulators folder
        /// </summary>
        private string _emulatorPath;

        /// <summary>
        /// The full path for the console preferences file
        /// </summary>
        private string _preferencesPath;

        /// <summary>
        /// The full path to the rom directory for the current console
        /// </summary>
        private string _romPath;

        /// <summary>
        /// The extensions for the current console
        /// </summary>
        private string _romExtensions;

        /// <summary>
        /// Basic console description and info
        /// </summary>
        private string _consoleInfo;

        /// <summary>
        /// The launch params for the current emulator
        /// </summary>
        private string _launchParams;

        /// <summary>
        /// A list of game objects for the current console instance
        /// </summary>
        private List<IGame> _gameList;

        /// <summary>
        /// The current game count for the console
        /// </summary>
        private int _gameCount;

        #endregion

        #region Constructors 

        /// <summary>
        /// Basic constructor for a new game console
        /// </summary>
        public Console(string consoleName)
        {
            ConsoleName = consoleName;
            _gameList = new List<IGame>();
        }

        /// <summary>
        /// Full constructor for creating a new game console
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emuPath"></param>
        /// <param name="romPath"></param>
        /// <param name="prefPath"></param>
        /// <param name="romExt"></param>
        /// <param name="gameCount"></param>
        /// <param name="consoleInfo"></param>
        /// <param name="launchParam"></param>
        /// <param name="releaseDate"></param>
        public Console(string name, string emuPath, string romPath, string prefPath, string romExt, int gameCount, string consoleInfo, string launchParam, string releaseDate)
        {
            ConsoleName = name;
            EmulatorPath = emuPath;
            RomPath = romPath;
            PreferencesPath = prefPath;
            RomExtension = romExt;
            _gameCount = gameCount;
            ConsoleInfo = consoleInfo;
            LaunchParams = launchParam;
            ReleaseDate = releaseDate;
            _gameList = new List<IGame>();
        }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Add a new game to the current console
        /// </summary>
        /// <param name="game"></param>
        /// <returns>'true' if the game was sucuessfully added</returns>
        public bool AddGame(IGame game)
        {
            //If the game console does not match the current console, return false
            if (!game.ConsoleName.Equals(ConsoleName))
            {
                return false;
            }

            //Verify that the game count does not exceed the max value
            if (_gameCount >= ConstValues.MAX_GAME_COUNT)
            {
                return false;
            }

            //If a game with an identical title (or filename) name already exists, return false
            if (_gameList.Find(e => e.Title.Equals(game.Title)) != null)
            {
                return false;
            }

            //If all conditions are valid, add the game and increment the game count for both the console and database 
            _gameList.Add(game);
            _gameCount++;
            return true;
        }

        /// <summary>
        /// Removes a game with the specified title from the Console
        /// Returns false if the fame title does not exist
        /// </summary>
        /// <param name="gameTitle"></param>
        /// <returns></returns>
        public bool RemoveGame(string gameTitle)
        {
            //Attempt to fetch the console from the current list
            IGame game = _gameList.Find(e => e.ConsoleName.Equals(gameTitle));

            if (game != null)
            {
                _gameList.Remove(game);
                _gameCount--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the IGame object with the specified title
        /// </summary>
        /// <param name="gameTitle">The title of the game to fetch</param>
        /// <returns>IGame object with the matching title</returns>
        public IGame GetGame(string gameTitle)
        {
            return _gameList.Find(c => c.Title.Equals(gameTitle));
        }

        /// <summary>
        /// Return a string list of all game titles
        /// </summary>
        /// <returns></returns>
        public List<string> GetGameList()
        {
            return _gameList.Select(g => g.Title).ToList();
        }

        /// <summary>
        /// Return the current number of games in the console
        /// </summary>
        /// <returns>the current game count</returns>
        public int GetGameCount()
        {
            return _gameCount;
        }

        #endregion
    }
}