﻿// 
// DiagramLibraryNode.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Xml;

namespace com.jonthysell.Chordious.Core
{
    internal class DiagramLibraryNode
    {
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = PathUtils.Clean(value);
            }
        }
        private string _path;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }

                _name = value.Trim();
            }
        }
        private string _name;

        public DiagramCollection DiagramCollection { get; private set; }

        private DiagramLibraryNode(DiagramStyle parentStyle)
        {
            this.DiagramCollection = new DiagramCollection(parentStyle);
        }

        public DiagramLibraryNode(DiagramStyle parentStyle, string path, string name) : this(parentStyle)
        {
            this.Path = path;
            this.Name = name;
        }

        public DiagramLibraryNode(DiagramStyle parentStyle, XmlReader xmlReader) : this(parentStyle)
        {
            this.Read(xmlReader);
        }

        public void Read(XmlReader xmlReader)
        {
            if (null == xmlReader)
            {
                throw new ArgumentNullException("xmlReader");
            }

            using (xmlReader)
            {
                if (xmlReader.IsStartElement() && xmlReader.Name == "diagrams")
                {
                    this.Path = xmlReader.GetAttribute("path");
                    this.Name = xmlReader.GetAttribute("name");

                    this.DiagramCollection.Read(xmlReader.ReadSubtree());
                }
            }
        }

        public void Write(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
            {
                throw new ArgumentNullException("xmlWriter");
            }

            xmlWriter.WriteStartElement("diagrams");

            xmlWriter.WriteAttributeString("name", this.Name);
            xmlWriter.WriteAttributeString("path", this.Path);

            this.DiagramCollection.Write(xmlWriter);

            xmlWriter.WriteEndElement();
        }
    }
}