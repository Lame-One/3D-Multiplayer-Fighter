﻿using System.Xml;
using System.Xml.Serialization;

public class Monster
{
	[XmlAttribute("name")]
	public string Name;

	public int Health;
}