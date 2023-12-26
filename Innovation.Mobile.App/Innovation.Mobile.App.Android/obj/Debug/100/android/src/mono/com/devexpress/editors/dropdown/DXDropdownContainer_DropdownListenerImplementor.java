package mono.com.devexpress.editors.dropdown;


public class DXDropdownContainer_DropdownListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.dropdown.DXDropdownContainer.DropdownListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_dropdownClosed:()V:GetDropdownClosedHandler:DevExpress.Xamarin.Android.Dropdown.DXDropdownContainer/IDropdownListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_dropdownOpened:()V:GetDropdownOpenedHandler:DevExpress.Xamarin.Android.Dropdown.DXDropdownContainer/IDropdownListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_dropdownResized:()V:GetDropdownResizedHandler:DevExpress.Xamarin.Android.Dropdown.DXDropdownContainer/IDropdownListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_dropdownWillClose:()Z:GetDropdownWillCloseHandler:DevExpress.Xamarin.Android.Dropdown.DXDropdownContainer/IDropdownListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"n_dropdownWillOpen:()Z:GetDropdownWillOpenHandler:DevExpress.Xamarin.Android.Dropdown.DXDropdownContainer/IDropdownListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.Xamarin.Android.Dropdown.DXDropdownContainer+IDropdownListenerImplementor, DevExpress.Xamarin.Android.Editors", DXDropdownContainer_DropdownListenerImplementor.class, __md_methods);
	}


	public DXDropdownContainer_DropdownListenerImplementor ()
	{
		super ();
		if (getClass () == DXDropdownContainer_DropdownListenerImplementor.class) {
			mono.android.TypeManager.Activate ("DevExpress.Xamarin.Android.Dropdown.DXDropdownContainer+IDropdownListenerImplementor, DevExpress.Xamarin.Android.Editors", "", this, new java.lang.Object[] {  });
		}
	}


	public void dropdownClosed ()
	{
		n_dropdownClosed ();
	}

	private native void n_dropdownClosed ();


	public void dropdownOpened ()
	{
		n_dropdownOpened ();
	}

	private native void n_dropdownOpened ();


	public void dropdownResized ()
	{
		n_dropdownResized ();
	}

	private native void n_dropdownResized ();


	public boolean dropdownWillClose ()
	{
		return n_dropdownWillClose ();
	}

	private native boolean n_dropdownWillClose ();


	public boolean dropdownWillOpen ()
	{
		return n_dropdownWillOpen ();
	}

	private native boolean n_dropdownWillOpen ();

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
