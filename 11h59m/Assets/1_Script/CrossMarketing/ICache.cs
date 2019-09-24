using UnityEngine;
using System.Collections;

public interface ICache
{
	void Put(string key, Texture2D texture);
	void Put(string key, string text);

	Texture2D GetTexture(string key);
	string GetText(string key);

	bool Delete(string key);
	void DeleteAll();
}