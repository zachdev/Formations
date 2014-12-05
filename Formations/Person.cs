using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class Person
    {
        private String _name;
        private String _password;

        public String Name 
        { 
            get 
            { 
                return _name; 
            } 
            private set 
            { 
                _name = value; 
            } 
        }
        public String Password 
        { 
            get 
            { 
                return _password; 
            } 
            private set 
            { 
                _password = value; 
            } 
        }
        public Person(String name, String password)
        {
            this.Name = name;
            this.Password = password;
        }
    }
}
