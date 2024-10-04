using System;
using System.Text;

namespace Project.Core.SourceGeneratorModule.Editor
{
    public class PropertyInstance
    {
        private AccessType _accessType = AccessType.None;
        private ModifierKeyWord _modifierKeyWord = ModifierKeyWord.None;
        private Type _propertyType = typeof(void);
        private string _name;
        private bool _hasGetter = true;
        private bool _hasSetter = false;
        private AccessType _getAccessType = AccessType.None;
        private AccessType _setAccessType = AccessType.None;
        private string _getterBody;
        private string _setterBody;
        private string _assignedDefaultValue;

        public string GetString()
        {
            var propertyStringBuilder = new StringBuilder();
        
            if (_accessType != AccessType.None)
                propertyStringBuilder.Append(_accessType.ToString().ToLower() + " ");
        
            if (_modifierKeyWord != ModifierKeyWord.None)
                propertyStringBuilder.Append(_modifierKeyWord.ToString().ToLower() + " ");
        
            propertyStringBuilder.Append(_propertyType + " ");
            propertyStringBuilder.Append(_name);
        
            propertyStringBuilder.Append(ToolConstants.OpenBracer);
        
            if (_hasGetter) 
            {
                if (_getAccessType != AccessType.None)
                    propertyStringBuilder.Append(_getAccessType.ToString().ToLower() + " ");

                propertyStringBuilder.Append("get");
                if (!string.IsNullOrEmpty(_getterBody))
                {
                    propertyStringBuilder.Append(ToolConstants.OpenBracer + "\n" + _getterBody + "\n" + ToolConstants.CloseBracer);
                } 
                else 
                {
                    propertyStringBuilder.Append(ToolConstants.SemiColon);
                }
            }
        
            if (_hasSetter)
            {
                if (_setAccessType != AccessType.None)
                    propertyStringBuilder.Append(_setAccessType.ToString().ToLower() + " ");

                propertyStringBuilder.Append("set");
                if (!string.IsNullOrEmpty(_setterBody))
                {
                    propertyStringBuilder.Append(ToolConstants.OpenBracer + "\n" + _setterBody + "\n" + ToolConstants.CloseBracer);
                } 
                else 
                {
                    propertyStringBuilder.Append(ToolConstants.SemiColon);
                }
            }
        
            propertyStringBuilder.Append(ToolConstants.CloseBracer);
        
            if (!string.IsNullOrEmpty(_assignedDefaultValue))
                propertyStringBuilder.Append(ToolConstants.EqualsSign + _assignedDefaultValue + ToolConstants.SemiColon);
            else
                propertyStringBuilder.Append(ToolConstants.SemiColon);

            return propertyStringBuilder.ToString();
        }

        public PropertyInstance Copy() =>
            new() {
                _accessType = _accessType,
                _modifierKeyWord = _modifierKeyWord,
                _propertyType = _propertyType,
                _name = _name,
                _hasGetter = _hasGetter,
                _hasSetter = _hasSetter,
                _getAccessType = _getAccessType,
                _setAccessType = _setAccessType,
                _getterBody = _getterBody,
                _setterBody = _setterBody,
                _assignedDefaultValue = _assignedDefaultValue
            };

        public PropertyInstance SetAccessType(AccessType accessType) 
        {
            _accessType = accessType;
            return this;
        }

        public PropertyInstance SetPublic() 
        {
            _accessType = AccessType.Public;
            return this;
        }

        public PropertyInstance SetPrivate() 
        {
            _accessType = AccessType.Private;
            return this;
        }

        public PropertyInstance SetProtected() 
        {
            _accessType = AccessType.Protected;
            return this;
        }

        public PropertyInstance SetInternal() 
        {
            _accessType = AccessType.Internal;
            return this;
        }

        public PropertyInstance SetModifierKeyWord(ModifierKeyWord modifierKeyWord) 
        {
            _modifierKeyWord = modifierKeyWord;
            return this;
        }

        public PropertyInstance SetStatic()
        {
            _modifierKeyWord = ModifierKeyWord.Static;
            return this;
        }

        public PropertyInstance SetConst() 
        {
            _modifierKeyWord = ModifierKeyWord.Const;
            return this;
        }

        public PropertyInstance SetAbstract() 
        {
            _modifierKeyWord = ModifierKeyWord.Abstract;
            return this;
        }

        public PropertyInstance SetVirtual() 
        {
            _modifierKeyWord = ModifierKeyWord.Virtual;
            return this;
        }

        public PropertyInstance SetOverride() 
        {
            _modifierKeyWord = ModifierKeyWord.Override;
            return this;
        }

        public PropertyInstance SetType(Type type) 
        {
            _propertyType = type;
            return this;
        }

        public PropertyInstance SetStringType() 
        {
            _propertyType = typeof(string);
            return this;
        }

        public PropertyInstance SetIntType() 
        {
            _propertyType = typeof(int);
            return this;
        }

        public PropertyInstance SetFloatType() 
        {
            _propertyType = typeof(float);
            return this;
        }

        public PropertyInstance SetDoubleType() 
        {
            _propertyType = typeof(double);
            return this;
        }

        public PropertyInstance SetName(string name) 
        {
            _name = name;
            return this;
        }

        public PropertyInstance SetHasGetter(bool hasGetter)
        {
            _hasGetter = hasGetter;
            return this;
        }

        public PropertyInstance SetHasSetter(bool hasSetter) 
        {
            _hasSetter = hasSetter;
            return this;
        }

        public PropertyInstance SetGetAccessType(AccessType getAccessType)
        {
            _getAccessType = getAccessType;
            return this;
        }

        public PropertyInstance SetSetAccessType(AccessType setAccessType)
        {
            _setAccessType = setAccessType;
            return this;
        }

        public PropertyInstance SetGetterBody(string getterBody)
        {
            _getterBody = getterBody;
            return this;
        }

        public PropertyInstance SetSetterBody(string setterBody) 
        {
            _setterBody = setterBody;
            return this;
        }

        public PropertyInstance SetAssignedDefaultValue(string defaultValue) 
        {
            _assignedDefaultValue = defaultValue;
            return this;
        }
    }
}