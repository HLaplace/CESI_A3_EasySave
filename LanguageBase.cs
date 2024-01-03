namespace classe
{
    public class LanguageBase
    {
        // Utilisez une variable privée pour stocker la valeur de la propriété
        private bool _cutcopy;
        public virtual bool cutcopy
        {
            get { return _cutcopy;  }
            set { _cutcopy = value; }
        }

        public virtual string setlang() { return ""; }
        public virtual string choicecutcopy() { return ""; }
        public virtual string validationchoicecut(){ return "";}
        public virtual string validationchoicecopy(){return "";}
        public virtual string choicestainname() { return ""; }
        public virtual string inputmessages(){return "";}
        public virtual string inputmessage() { return ""; }
        public virtual string inputerrormessages(){return "";}
    
}
}
