package crc6439a491d224ce8c87;


public abstract class XamarinDataFormAbstractEditorBase
	extends com.devexpress.editors.dataForm.protocols.DXDataFormEditorItem
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_commitEditorValue:()Z:GetCommitEditorValueHandler\n" +
			"n_validateEditorValue:()Z:GetValidateEditorValueHandler\n" +
			"n_resetEditorValue:()V:GetResetEditorValueHandler\n" +
			"n_getEditorWrappedValue:()Ljava/lang/Object;:GetGetEditorWrappedValueHandler\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormAbstractEditorBase, DevExpress.XamarinForms.Editors.Android", XamarinDataFormAbstractEditorBase.class, __md_methods);
	}


	public XamarinDataFormAbstractEditorBase (android.view.View p0)
	{
		super (p0);
		if (getClass () == XamarinDataFormAbstractEditorBase.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormAbstractEditorBase, DevExpress.XamarinForms.Editors.Android", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public XamarinDataFormAbstractEditorBase (android.view.View p0, boolean p1)
	{
		super (p0, p1);
		if (getClass () == XamarinDataFormAbstractEditorBase.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormAbstractEditorBase, DevExpress.XamarinForms.Editors.Android", "Android.Views.View, Mono.Android:System.Boolean, mscorlib", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public XamarinDataFormAbstractEditorBase (android.view.View p0, boolean p1, com.devexpress.editors.dataForm.protocols.DXDataFormEditorItemErrorProvider p2)
	{
		super (p0, p1, p2);
		if (getClass () == XamarinDataFormAbstractEditorBase.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormAbstractEditorBase, DevExpress.XamarinForms.Editors.Android", "Android.Views.View, Mono.Android:System.Boolean, mscorlib:Com.Devexpress.Editors.DataForm.Protocols.DXDataFormEditorItemErrorProvider, DevExpress.Xamarin.Android.Editors", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public boolean commitEditorValue ()
	{
		return n_commitEditorValue ();
	}

	private native boolean n_commitEditorValue ();


	public boolean validateEditorValue ()
	{
		return n_validateEditorValue ();
	}

	private native boolean n_validateEditorValue ();


	public void resetEditorValue ()
	{
		n_resetEditorValue ();
	}

	private native void n_resetEditorValue ();


	public java.lang.Object getEditorWrappedValue ()
	{
		return n_getEditorWrappedValue ();
	}

	private native java.lang.Object n_getEditorWrappedValue ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
