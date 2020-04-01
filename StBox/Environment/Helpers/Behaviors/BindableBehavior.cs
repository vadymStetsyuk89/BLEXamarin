using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StBox.Environment.Helpers.Behaviors
{
    public class BindableBehavior<T> : Behavior<T> 
        where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T visualElememt)
        {
            base.OnAttachedTo(visualElememt);

            AssociatedObject = visualElememt;

            if (visualElememt.BindingContext != null)
            {
                BindingContext = visualElememt.BindingContext;
            }

            visualElememt.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T view)
        {
            view.BindingContextChanged -= OnBindingContextChanged;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
    }
}
