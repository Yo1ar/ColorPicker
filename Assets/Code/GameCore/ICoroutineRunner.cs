using System.Collections;
using UnityEngine;

namespace GameCore
{
	public interface ICoroutineRunner
	{
		public Coroutine StartCoroutine(IEnumerator coroutine);
	}
}