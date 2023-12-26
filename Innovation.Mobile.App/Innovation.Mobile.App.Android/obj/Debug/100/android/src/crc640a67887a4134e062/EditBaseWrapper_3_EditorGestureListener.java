package crc640a67887a4134e062;


public class EditBaseWrapper_3_EditorGestureListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.EditorGestureListener,
		com.devexpress.editors.BaseGestureListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDoubleTap:()Z:GetOnDoubleTapHandler:DevExpress.Xamarin.Android.Editors.IBaseGestureListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onLongPress:()Z:GetOnLongPressHandler:DevExpress.Xamarin.Android.Editors.IBaseGestureListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onSingleTapUp:()Z:GetOnSingleTapUpHandler:DevExpress.Xamarin.Android.Editors.IBaseGestureListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Wrappers.EditBaseWrapper`3+EditorGestureListener, DevExpress.XamarinForms.Editors.Android", EditBaseWrapper_3_EditorGestureListener.class, __md_methods);
	}


	public EditBaseWrapper_3_EditorGestureListener ()
	{
		super ();
		if (getClass () == EditBaseWrapper_3_EditorGestureListener.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Wrappers.EditBaseWrapper`3+EditorGestureListener, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public boolean onDoubleTap ()
	{
		return n_onDoubleTap ();
	}

	private native boolean n_onDoubleTap ();


	public boolean onLongPress ()
	{
		return n_onLongPress ();
	}

	private native boolean n_onLongPress ();


	public boolean onSingleTapUp ()
	{
		return n_onSingleTapUp ();
	}

	private native boolean n_onSingleTapUp ();

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
