package crc64e58ce59a6d1a044d;


public class TextEditRendererBase_3
	extends crc64e58ce59a6d1a044d.EditRendererBase_3
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.OnEditActionListener,
		com.devexpress.editors.OnTextChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onEditorAction:(Lcom/devexpress/editors/TextEditBase;ILandroid/view/KeyEvent;)Z:GetOnEditorAction_Lcom_devexpress_editors_TextEditBase_ILandroid_view_KeyEvent_Handler:DevExpress.Xamarin.Android.Editors.IOnEditActionListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onTextChanged:(Lcom/devexpress/editors/TextEditBase;Ljava/lang/CharSequence;)V:GetOnTextChanged_Lcom_devexpress_editors_TextEditBase_Ljava_lang_CharSequence_Handler:DevExpress.Xamarin.Android.Editors.IOnTextChangedListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.TextEditRendererBase`3, DevExpress.XamarinForms.Editors.Android", TextEditRendererBase_3.class, __md_methods);
	}


	public TextEditRendererBase_3 (android.content.Context p0)
	{
		super (p0);
		if (getClass () == TextEditRendererBase_3.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.TextEditRendererBase`3, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public TextEditRendererBase_3 (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == TextEditRendererBase_3.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.TextEditRendererBase`3, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public TextEditRendererBase_3 (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == TextEditRendererBase_3.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.TextEditRendererBase`3, DevExpress.XamarinForms.Editors.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public boolean onEditorAction (com.devexpress.editors.TextEditBase p0, int p1, android.view.KeyEvent p2)
	{
		return n_onEditorAction (p0, p1, p2);
	}

	private native boolean n_onEditorAction (com.devexpress.editors.TextEditBase p0, int p1, android.view.KeyEvent p2);


	public void onTextChanged (com.devexpress.editors.TextEditBase p0, java.lang.CharSequence p1)
	{
		n_onTextChanged (p0, p1);
	}

	private native void n_onTextChanged (com.devexpress.editors.TextEditBase p0, java.lang.CharSequence p1);

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
