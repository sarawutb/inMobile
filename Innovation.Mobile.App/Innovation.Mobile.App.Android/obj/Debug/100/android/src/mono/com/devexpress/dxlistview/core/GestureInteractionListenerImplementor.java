package mono.com.devexpress.dxlistview.core;


public class GestureInteractionListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxlistview.core.GestureInteractionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_down:(Landroid/view/MotionEvent;)Z:GetDown_Landroid_view_MotionEvent_Handler:Com.Devexpress.Dxlistview.Core.IGestureInteractionListenerInvoker, DevExpress.Xamarin.Android.CollectionView\n" +
			"n_move:(Landroid/view/MotionEvent;)Z:GetMove_Landroid_view_MotionEvent_Handler:Com.Devexpress.Dxlistview.Core.IGestureInteractionListenerInvoker, DevExpress.Xamarin.Android.CollectionView\n" +
			"n_onLongPress:(Landroid/view/MotionEvent;)Z:GetOnLongPress_Landroid_view_MotionEvent_Handler:Com.Devexpress.Dxlistview.Core.IGestureInteractionListenerInvoker, DevExpress.Xamarin.Android.CollectionView\n" +
			"n_onSingleTapUp:(Landroid/view/MotionEvent;)Z:GetOnSingleTapUp_Landroid_view_MotionEvent_Handler:Com.Devexpress.Dxlistview.Core.IGestureInteractionListenerInvoker, DevExpress.Xamarin.Android.CollectionView\n" +
			"n_up:(Landroid/view/MotionEvent;)Z:GetUp_Landroid_view_MotionEvent_Handler:Com.Devexpress.Dxlistview.Core.IGestureInteractionListenerInvoker, DevExpress.Xamarin.Android.CollectionView\n" +
			"";
		mono.android.Runtime.register ("Com.Devexpress.Dxlistview.Core.IGestureInteractionListenerImplementor, DevExpress.Xamarin.Android.CollectionView", GestureInteractionListenerImplementor.class, __md_methods);
	}


	public GestureInteractionListenerImplementor ()
	{
		super ();
		if (getClass () == GestureInteractionListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Com.Devexpress.Dxlistview.Core.IGestureInteractionListenerImplementor, DevExpress.Xamarin.Android.CollectionView", "", this, new java.lang.Object[] {  });
		}
	}


	public boolean down (android.view.MotionEvent p0)
	{
		return n_down (p0);
	}

	private native boolean n_down (android.view.MotionEvent p0);


	public boolean move (android.view.MotionEvent p0)
	{
		return n_move (p0);
	}

	private native boolean n_move (android.view.MotionEvent p0);


	public boolean onLongPress (android.view.MotionEvent p0)
	{
		return n_onLongPress (p0);
	}

	private native boolean n_onLongPress (android.view.MotionEvent p0);


	public boolean onSingleTapUp (android.view.MotionEvent p0)
	{
		return n_onSingleTapUp (p0);
	}

	private native boolean n_onSingleTapUp (android.view.MotionEvent p0);


	public boolean up (android.view.MotionEvent p0)
	{
		return n_up (p0);
	}

	private native boolean n_up (android.view.MotionEvent p0);

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
