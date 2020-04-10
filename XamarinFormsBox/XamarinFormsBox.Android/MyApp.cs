using Android.App;
using Android.Runtime;
using Firebase;
using System;

namespace XamarinFormsBox.Droid
{

#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public class MayApp : Application
    {
        public MayApp(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();

            FirebaseApp.InitializeApp(this);
        }
    }
}