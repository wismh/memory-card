using System;
using System.Text;

namespace Project.Core.SourceGeneratorModule.Editor
{
    public class FieldInstance 
    {
        private AccessType _accessType = AccessType.None;

        private ModifierKeyWord _modifierKeyWord = ModifierKeyWord.None;

        private string _fieldType = typeof(void).Name;

        private string _name;

        private string _assignedValue;

        public string GetString() 
        {
            var fieldStringBuilder = new StringBuilder();
            if (_accessType != AccessType.None)
                fieldStringBuilder.Append(_accessType.ToString().ToLower() + " ");

            if (_modifierKeyWord != ModifierKeyWord.None)
                fieldStringBuilder.Append(_modifierKeyWord.ToString().ToLower() + " ");

            fieldStringBuilder.Append(_fieldType + " ");
            fieldStringBuilder.Append(_name);

            if (string.IsNullOrEmpty(_assignedValue) is false)
                fieldStringBuilder.Append(ToolConstants.EqualsSign + _assignedValue);

            fieldStringBuilder.Append(ToolConstants.SemiColon);
            return fieldStringBuilder.ToString();
        }

        public FieldInstance Copy() =>
            new() {
                _accessType = _accessType,
                _modifierKeyWord = _modifierKeyWord,
                _fieldType = _fieldType,
                _name = _name,
                _assignedValue = _assignedValue
            };

        public FieldInstance SetAccessType(AccessType accessType)
        {
            _accessType = accessType;
            return this;
        }

        public FieldInstance SetPublic() 
        {
            _accessType = AccessType.Public;
            return this;
        }

        public FieldInstance SetPrivate() 
        {
            _accessType = AccessType.Private;
            return this;
        }

        public FieldInstance SetProtected() 
        {
            _accessType = AccessType.Protected;
            return this;
        }

        public FieldInstance SetInternal() 
        {
            _accessType = AccessType.Internal;
            return this;
        }

        public FieldInstance SetModifierKeyWord(ModifierKeyWord modifierKeyWord)
        {
            _modifierKeyWord = modifierKeyWord;
            return this;
        }

        public FieldInstance SetStatic() 
        {
            _modifierKeyWord = ModifierKeyWord.Static;
            return this;
        }

        public FieldInstance SetConst()
        {
            _modifierKeyWord = ModifierKeyWord.Const;
            return this;
        }

        public FieldInstance SetAbstract() 
        {
            _modifierKeyWord = ModifierKeyWord.Abstract;
            return this;
        }

        public FieldInstance SetVirtual()
        {
            _modifierKeyWord = ModifierKeyWord.Virtual;
            return this;
        }

        public FieldInstance SetOverride() 
        {
            _modifierKeyWord = ModifierKeyWord.Override;
            return this;
        }

        public FieldInstance SetType(Type type) 
        {
            _fieldType = type.FullName;
            return this;
        }

        public FieldInstance SetStringType()
        {
            _fieldType = typeof(string).FullName;
            return this;
        }

        public FieldInstance SetIntType() 
        {
            _fieldType = typeof(int).FullName;
            return this;
        }

        public FieldInstance SetFloatType() 
        {
            _fieldType = typeof(float).FullName;
            return this;
        }

        public FieldInstance SetDoubleType() 
        {
            _fieldType = typeof(double).FullName;
            return this;
        }

        public FieldInstance SetListType(string listVariablesType)
        {
            _fieldType = $"List<{listVariablesType}>";
            return this;
        }

        public FieldInstance SetName(string name)
        {
            _name = name;
            return this;
        }

        public FieldInstance SetAssignedValue(string assignedValue) 
        {
            _assignedValue = assignedValue;
            return this;
        }
    }
}