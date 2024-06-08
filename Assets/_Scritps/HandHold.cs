using UnityEngine.XR.Interaction.Toolkit;

public class HandHold : XRBaseInteractable
{
    protected Weapon gun = null;

    public void Setup(Weapon gun)
    {
        this.gun = gun;
    }

    protected override void Awake()
    {
        base.Awake();
        onActivate.AddListener(BeginAction);
        onDeactivate.AddListener(EndAction);
        onSelectEntered.AddListener(Grab);
        onSelectEntered.AddListener(Drop);
    }

    private void OnDestroy()
    {
        onActivate.RemoveListener(BeginAction);
        onDeactivate.RemoveListener(EndAction);
        onSelectEntered.RemoveListener(Grab);
        onSelectEntered.RemoveListener(Drop);
    }

    protected virtual void Grab(XRBaseInteractor interactor)
    {

    }

        protected virtual void Drop(XRBaseInteractor interactor)
    {

    }



    protected virtual void BeginAction(XRBaseInteractor interactor)
    {
        //Empty
    }

    protected virtual void EndAction(XRBaseInteractor interactor)
    {
        //Empty
    }



}
