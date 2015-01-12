using UnityEngine;
using System.Collections;

using UnityObject = UnityEngine.Object;

public abstract class DatabaseBase<T> : ManagerBase<T> where T : UnityObject
{
    #region Variables / Properties

    public bool UseExistingList = false;
    public bool TryDownloadingBlob = false;
    public bool IsLoaded = false;

    public TextAsset LocalBlob;
    public string RemoteBlobUrl;
    public string RawBlob;

    #endregion Variables / Properties

    #region Hooks

    public virtual void Start()
    {
        if (UseExistingList)
            return;

        LoadItemsFromJson();
    }

    #endregion Hooks

    #region Methods

    public virtual void LoadItemsFromJson()
    {
        if (TryDownloadingBlob)
            StartCoroutine(DownloadBlob());

        if (string.IsNullOrEmpty(RawBlob))
        {
            RawBlob = FetchLocalBlob();

            if (string.IsNullOrEmpty(RawBlob))
                return;
            else
                MapBlob();
        }
        else
        {
            MapBlob();
        }
    }

    protected virtual string FetchLocalBlob()
    {
        if (LocalBlob == null)
            return string.Empty;

        return LocalBlob.text;
    }

    protected virtual IEnumerator DownloadBlob()
    {
        if (!TryDownloadingBlob)
            yield break;

        WWW dataInterface = new WWW(RemoteBlobUrl);

        while (!dataInterface.isDone)
            yield return null;

        RawBlob = dataInterface.text;

        // TODO: Check that the blob has not been corrupted.
        //       If it has, set the raw blob to empty.

        dataInterface.Dispose();
    }

    protected abstract void MapBlob();

    #endregion Methods
}
