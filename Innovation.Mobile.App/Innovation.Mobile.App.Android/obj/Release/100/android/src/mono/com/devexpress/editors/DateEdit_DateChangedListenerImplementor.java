package mono.com.devexpress.editors;


public class DateEdit_DateChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.editors.DateEdit.DateChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDateChanged:(Lcom/devexpress/editors/DateEdit;III)V:GetOnDateChanged_Lcom_devexpress_editors_DateEdit_IIIHandler:DevExpress.Xamarin.Android.Editors.DateEdit/IDateChangedListenerInvoker, DevExpress.Xamarin.Android.Editors\n" +
			"";
		mono.android.Runtime.register ("DevExpress.Xamarin.Android.Editors.DateEdit+IDateChangedListenerImplementor, DevExpress.Xamarin.Android.Editors", DateEdit_DateChangedListenerImplementor.class, __md_methods);
	}


	public DateEdit_DateChangedListenerImplementor ()
	{
		super ();
		if (getClass () == DateEdit_DateChangedListenerImplementor.class) {
			mono.android.TypeManager.Activate ("DevExpress.Xamarin.Android.Editors.DateEdit+IDateChangedListenerImplementor, DevExpress.Xamarin.Android.Editors", "", this, new java.lang.Object[] {  });
		}
	}


	public void onDateChanged (com.devexpress.editors.DateEdit p0, int p1, int p2, int p3)
	{
		n_onDateChanged (p0, p1, p2, p3);
	}

	private native void n_onDateChanged (com.devexpress.editors.DateEdit p0, int p1, int p2, int p3);

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
