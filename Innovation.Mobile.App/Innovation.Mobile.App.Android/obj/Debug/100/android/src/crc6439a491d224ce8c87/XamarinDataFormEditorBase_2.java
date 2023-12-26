package crc6439a491d224ce8c87;


public abstract class XamarinDataFormEditorBase_2
	extends crc6439a491d224ce8c87.XamarinDataFormAbstractEditorBase
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getView:()Landroid/view/View;:GetGetViewHandler\n" +
			"n_getEdit:()Lcom/devexpress/editors/EditBase;:GetGetEditHandler\n" +
			"n_configure:()V:GetConfigureHandler\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormEditorBase`2, DevExpress.XamarinForms.Editors.Android", XamarinDataFormEditorBase_2.class, __md_methods);
	}


	public XamarinDataFormEditorBase_2 (android.view.View p0)
	{
		super (p0);
		if (getClass () == XamarinDataFormEditorBase_2.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormEditorBase`2, DevExpress.XamarinForms.Editors.Android", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public XamarinDataFormEditorBase_2 (android.view.View p0, boolean p1)
	{
		super (p0, p1);
		if (getClass () == XamarinDataFormEditorBase_2.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormEditorBase`2, DevExpress.XamarinForms.Editors.Android", "Android.Views.View, Mono.Android:System.Boolean, mscorlib", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public XamarinDataFormEditorBase_2 (android.view.View p0, boolean p1, com.devexpress.editors.dataForm.protocols.DXDataFormEditorItemErrorProvider p2)
	{
		super (p0, p1, p2);
		if (getClass () == XamarinDataFormEditorBase_2.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DataForm.Editors.XamarinDataFormEditorBase`2, DevExpress.XamarinForms.Editors.Android", "Android.Views.View, Mono.Android:System.Boolean, mscorlib:Com.Devexpress.Editors.DataForm.Protocols.DXDataFormEditorItemErrorProvider, DevExpress.Xamarin.Android.Editors", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public android.view.View getView ()
	{
		return n_getView ();
	}

	private native android.view.View n_getView ();


	public com.devexpress.editors.EditBase getEdit ()
	{
		return n_getEdit ();
	}

	private native com.devexpress.editors.EditBase n_getEdit ();


	public void configure ()
	{
		n_configure ();
	}

	private native void n_configure ();

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
