#region License
/*
MIT License

Copyright(c) 2017-2020 Mattias Edlund

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System;

namespace jp.lilxyzw.ndmfmeshsimplifier.UnityMeshSimplifier
{
    /// <summary>
    /// An exception thrown when validating simplification options.
    /// </summary>
    internal sealed class ValidateSimplificationOptionsException : Exception
    {
        private readonly string propertyName;

        /// <summary>
        /// Creates a new simplification options validation exception.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="message">The exception message.</param>
        public ValidateSimplificationOptionsException(string propertyName, string message)
            : base(message)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Creates a new simplification options validation exception.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The exception that caused the validation error.</param>
        public ValidateSimplificationOptionsException(string propertyName, string message, Exception innerException)
            : base(message, innerException)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Gets the property name that caused the validation error.
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Gets the message of the exception.
        /// </summary>
        public override string Message
        {
            get { return base.Message + Environment.NewLine + "Property name: " + propertyName; }
        }
    }
}
