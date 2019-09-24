using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Cache : ICache
{
	private const string ROOT_DIR = @"cache";

	// 캐시 폴더 이름
	private		string				mKey = null;

	private		string				rootPath {
		get { return System.IO.Path.Combine(Application.persistentDataPath, ROOT_DIR + "/" + mKey); }
	}

	public Cache(string key)
	{
		mKey = key;
		LoadOrReset();
	}

	void LoadOrReset()
	{
		if( !System.IO.Directory.Exists(rootPath) ) System.IO.Directory.CreateDirectory( rootPath );
	}

	public bool Delete(string key)
	{
		try {
			string path = System.IO.Path.Combine(rootPath, MyTools.Md5(key)); 
			if( System.IO.File.Exists(path))
			{
				System.IO.File.Delete( path );
				return true;
			}
			return false;
		} catch {
			return false;
		}
	}

	public void DeleteAll()
	{
		foreach( string fileName in System.IO.Directory.GetFiles(rootPath) )
		{
			System.IO.File.Delete( System.IO.Path.Combine(rootPath, fileName) );
		}
	}

	public void Put(string key, Texture2D texture)
	{
		string path = System.IO.Path.Combine(rootPath, MyTools.Md5(key));
		if( System.IO.File.Exists(path) ) Delete( key );

		byte[] bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(path, bytes );
	}

	public void Put(string key, string text)
	{
		string path = System.IO.Path.Combine(rootPath, MyTools.Md5(key));
		if( System.IO.File.Exists(path) ) Delete( key );

		byte[] bytes = System.Text.Encoding.UTF8.GetBytes( text );
		System.IO.File.WriteAllBytes( path, bytes );
	}

	public Texture2D GetTexture(string key)
	{
		string path = System.IO.Path.Combine(rootPath, MyTools.Md5(key));
		if( !System.IO.File.Exists(path) ) return null;

		try {
			byte[] bytes = System.IO.File.ReadAllBytes( path );
			
			Texture2D texture = new Texture2D(4, 4);
			texture.LoadImage(bytes);

			return texture;
		} catch {
			return null;
		}
	}

	public string GetText(string key)
	{
		string path = System.IO.Path.Combine(rootPath, MyTools.Md5(key));
		if( !System.IO.File.Exists(path) ) return null;

		try {
			byte[] bytes = System.IO.File.ReadAllBytes( path );
			return System.Text.Encoding.UTF8.GetString( bytes );
		} catch {
			return null;
		}
	}
}