using UnityEngine;
using System.Collections;

public class JsonBlobLoaderBase : DebuggableBehavior
{
	#region Variables / Properties

	public bool TryDownloadingBlob = false;

	public TextAsset LocalBlob;
	public string RemoteBlobUrl;
	public string RawBlob;

	#endregion Variables / Properties

	#region Methods

	protected IEnumerator DownloadBlob()
	{
		if(! TryDownloadingBlob)
			yield break;
		
		WWW dataInterface = new WWW(RemoteBlobUrl);
		
		while(! dataInterface.isDone)
			yield return 0;
		
		RawBlob = dataInterface.text;
		
		// TODO: Check that the blob has not been corrupted.
		//       If it has, set the raw blob to empty.
		
		dataInterface.Dispose();
	}
	
	protected string FetchLocalBlob()
	{
		if(LocalBlob == null)
			return string.Empty;
		
		return LocalBlob.text;
	}

	#endregion Methods
}
