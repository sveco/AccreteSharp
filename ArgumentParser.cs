/////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2007                                                      //
//                                                                         //
// This code is free software under the Creative Commons licence           //
// Attribution-Noncommercial-Share Alike 2.0 Generic                       //
// http://creativecommons.org/licenses/by-nc-sa/2.0/                       //
//                                                                         //          
// Janko Svetlik                                                           //
//                                                                         //
// http://sveco.aspweb.cz                                                  //
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AccreteSharp
{
    /// <summary>
    /// This class is a wrapper for getting a list of named arguments from command line 
    /// </summary>
    class ArgumentParser
    {
        #region Variables    ------------------------------------------------
        IList<Argument> _argCollection = new List<Argument>();
        #endregion

        #region Constructors ------------------------------------------------
        public ArgumentParser()
        {
        }

        public ArgumentParser(string[] args)
        {
            int i;
            string arg;
            for(i = 0; i < args.Length; i++)
            {
                if (args.Length > i)
                {
                    arg = args[i];
                    if (arg.IndexOf("-") == 0 && arg.Length > 1)
                    {
                        if (args.Length > i + 1)
                        {
                            if(args[i+1].IndexOf("-") == -1)
                            {
                                _argCollection.Add(new Argument(args[i].Substring(1),args[i+1]));
                                i++;
                             }
                            else
                            {
                                _argCollection.Add(new Argument(args[i].Substring(1), "1"));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods      ------------------------------------------------
        public string GetArgValue(string argName)
        {
            string _retVal = "0";
            foreach (Argument arg in _argCollection)
            {
                if (argName == arg.Name)
                {
                    _retVal = arg.ValueStr;
                }
            }
            return _retVal;
        }
        #endregion
    }

    /// <summary>
    /// Encapsulates different types of arguments
    /// </summary>
    class Argument
    {
        #region Local variables ---------------------------------------------
        private string _argName;
        private string _argValueStr;
        private bool _argValueBool;
        private int _argValueInt;
        #endregion

        #region Constructors ------------------------------------------------
        public Argument(
            string name,
            string value
            )
        {
            this.Name = name;
            this.ValueStr = value;
        }

        public Argument(
            string name,
            bool value
            )
        {
            this.Name = name;
            this.ValueBool = value;
        }

        public Argument(
            string name,
            int value
            )
        {
            this.Name = name;
            this.ValueInt = value;
        }
        #endregion

        #region Properties   ------------------------------------------------
        public string Name
        {
            get { return _argName; }
            set { _argName = value; }
        }

        public string ValueStr
        {
            get { return _argValueStr; }
            set { _argValueStr = value; }
        }

        public bool ValueBool
        {
            get { return _argValueBool; }
            set { _argValueBool = value; }
        }

        public int ValueInt
        {
            get { return _argValueInt; }
            set { _argValueInt = value; }
        }
        #endregion
    }
}
