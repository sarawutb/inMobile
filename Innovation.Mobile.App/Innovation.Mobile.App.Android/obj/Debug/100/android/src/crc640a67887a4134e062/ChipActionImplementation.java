package crc640a67887a4134e062;


public class ChipActionImplementation
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.ChipAction,
		com.devexpress.editors.BaseGestureListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCloseIconTap:()Z:GetOnCloseIconTapHandler:DevExpress.Xamarin.Android.Editors.IChipActionInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onSingleTapConfirmed:()Z:GetOnSingleTapConfirmedHandler:DevExpress.Xamarin.Android.Editors.IChipActionInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onSizeChanged:()V:GetOnSizeChangedHandler:DevExpress.Xamarin.Android.Editors.IChipActionInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onDoubleTap:()Z:GetOnDoubleTapHandler:DevExpress.Xamarin.Android.Editors.IBaseGestureListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onLongPress:()Z:GetOnLongPressHandler:DevExpress.Xamarin.Android.Editors.IBaseGestureListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_onSingleTapUp:()Z:GetOnSingleTapUpHandler:DevExpress.Xamarin.Android.Editors.IBaseGestureListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Wrappers.ChipActionImplementation, DevExpress.XamarinForms.Editors.Android", ChipActionImplementation.class, __md_methods);
	}


	public ChipActionImplementation ()
	{
		super ();
		if (getClass () == ChipActionImplementation.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Wrappers.ChipActionImplementation, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public boolean onCloseIconTap ()
	{
		return n_onCloseIconTap ();
	}

	private native boolean n_onCloseIconTap ();


	public boolean onSingleTapConfirmed ()
	{
		return n_onSingleTapConfirmed ();
	}

	private native boolean n_onSingleTapConfirmed ();


	public void onSizeChanged ()
	{
		n_onSizeChanged ();
	}

	private native void n_onSizeChanged ();


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
