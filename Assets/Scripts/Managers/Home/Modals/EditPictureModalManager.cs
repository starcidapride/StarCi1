
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

using static PathUtility;
using static GameObjectUtility;

public class EditPictureModalManager : Singleton<EditPictureModalManager> 
{ 
    [SerializeField]
    private Transform content;

    [SerializeField]
    private Transform cellPicture;

    [SerializeField]
    private Button cancelButton;

    public void Start()
    {
        DestroyAllChildGameObjects(content);
        
        var cells = GetAllIndexedPictures();

        foreach (var cell in cells)
        {
            var cellInstance = Instantiate(cellPicture, content);

            var manager = cellInstance.GetComponent<CellPictureManager>();

            manager.SetImage(cell.Key, cell.Value);
        }

        cancelButton.onClick.AddListener(ModalManager.Instance.CloseNearestModal);
    }


}
