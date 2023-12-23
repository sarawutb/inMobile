package crc648a6528da278f03de;


public class DataFormDataProvider
	extends com.devexpress.editors.dataForm.protocols.DXDataFormDataProvider
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getEditors:(I)Ljava/util/List;:GetGetEditors_IHandler\n" +
			"n_getGroups:()Ljava/util/List;:GetGetGroupsHandler\n" +
			"n_isLastElementInLine:(II)Z:GetIsLastElementInLine_IIHandler\n" +
			"n_getEditor:(II)Lcom/devexpress/editors/dataForm/protocols/DataFormEditorInfo;:GetGetEditor_IIHandler\n" +
			"n_getEditorDisplayFormat:(II)Ljava/lang/String;:GetGetEditorDisplayFormat_IIHandler\n" +
			"n_getEditorDisplayText:(II)Ljava/lang/String;:GetGetEditorDisplayText_IIHandler\n" +
			"n_getEditorDisplayText:(IILjava/lang/Object;)Ljava/lang/String;:GetGetEditorDisplayText_IILjava_lang_Object_Handler\n" +
			"n_getEditorValue:(II)Ljava/lang/Object;:GetGetEditorValue_IIHandler\n" +
			"n_getErrorForEditor:(II)Lcom/devexpress/editors/dataForm/protocols/DataFormValidationError;:GetGetErrorForEditor_IIHandler\n" +
			"n_getGroup:(I)Lcom/devexpress/editors/dataForm/protocols/ExpanderInfo;:GetGetGroup_IHandler\n" +
			"n_getPickerDataSource:(II)[Ljava/lang/String;:GetGetPickerDataSource_IIHandler\n" +
			"n_isSourceUpdated:(II)Z:GetIsSourceUpdated_IIHandler\n" +
			"n_postValue:(Ljava/lang/Object;II)V:GetPostValue_Ljava_lang_Object_IIHandler\n" +
			"n_validateValue:(Ljava/lang/Object;II)Lcom/devexpress/editors/dataForm/protocols/DataFormValidationError;:GetValidateValue_Ljava_lang_Object_IIHandler\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Editors.Android.DataForm.DataFormDataProvider, DevExpress.XamarinForms.Editors.Android", DataFormDataProvider.class, __md_methods);
	}


	public DataFormDataProvider ()
	{
		super ();
		if (getClass () == DataFormDataProvider.class) {
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Editors.Android.DataForm.DataFormDataProvider, DevExpress.XamarinForms.Editors.Android", "", this, new java.lang.Object[] {  });
		}
	}


	public java.util.List getEditors (int p0)
	{
		return n_getEditors (p0);
	}

	private native java.util.List n_getEditors (int p0);


	public java.util.List getGroups ()
	{
		return n_getGroups ();
	}

	private native java.util.List n_getGroups ();


	public boolean isLastElementInLine (int p0, int p1)
	{
		return n_isLastElementInLine (p0, p1);
	}

	private native boolean n_isLastElementInLine (int p0, int p1);


	public com.devexpress.editors.dataForm.protocols.DataFormEditorInfo getEditor (int p0, int p1)
	{
		return n_getEditor (p0, p1);
	}

	private native com.devexpress.editors.dataForm.protocols.DataFormEditorInfo n_getEditor (int p0, int p1);


	public java.lang.String getEditorDisplayFormat (int p0, int p1)
	{
		return n_getEditorDisplayFormat (p0, p1);
	}

	private native java.lang.String n_getEditorDisplayFormat (int p0, int p1);


	public java.lang.String getEditorDisplayText (int p0, int p1)
	{
		return n_getEditorDisplayText (p0, p1);
	}

	private native java.lang.String n_getEditorDisplayText (int p0, int p1);


	public java.lang.String getEditorDisplayText (int p0, int p1, java.lang.Object p2)
	{
		return n_getEditorDisplayText (p0, p1, p2);
	}

	private native java.lang.String n_getEditorDisplayText (int p0, int p1, java.lang.Object p2);


	public java.lang.Object getEditorValue (int p0, int p1)
	{
		return n_getEditorValue (p0, p1);
	}

	private native java.lang.Object n_getEditorValue (int p0, int p1);


	public com.devexpress.editors.dataForm.protocols.DataFormValidationError getErrorForEditor (int p0, int p1)
	{
		return n_getErrorForEditor (p0, p1);
	}

	private native com.devexpress.editors.dataForm.protocols.DataFormValidationError n_getErrorForEditor (int p0, int p1);


	public com.devexpress.editors.dataForm.protocols.ExpanderInfo getGroup (int p0)
	{
		return n_getGroup (p0);
	}

	private native com.devexpress.editors.dataForm.protocols.ExpanderInfo n_getGroup (int p0);


	public java.lang.String[] getPickerDataSource (int p0, int p1)
	{
		return n_getPickerDataSource (p0, p1);
	}

	private native java.lang.String[] n_getPickerDataSource (int p0, int p1);


	public boolean isSourceUpdated (int p0, int p1)
	{
		return n_isSourceUpdated (p0, p1);
	}

	private native boolean n_isSourceUpdated (int p0, int p1);


	public void postValue (java.lang.Object p0, int p1, int p2)
	{
		n_postValue (p0, p1, p2);
	}

	private native void n_postValue (java.lang.Object p0, int p1, int p2);


	public com.devexpress.editors.dataForm.protocols.DataFormValidationError validateValue (java.lang.Object p0, int p1, int p2)
	{
		return n_validateValue (p0, p1, p2);
	}

	private native com.devexpress.editors.dataForm.protocols.DataFormValidationError n_validateValue (java.lang.Object p0, int p1, int p2);

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
