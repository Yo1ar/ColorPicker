using UnityEngine;

public interface IErasable
{
	Vector3 Position { get; }
	void Erase();
	void Highlight(bool value);
}