﻿using System;
using System.ComponentModel;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[ContentProperty("Content")]
	[RenderWith(typeof(_SwipeViewRenderer))]
	public class SwipeView : ContentView, IElementConfiguration<SwipeView>
	{
		readonly Lazy<PlatformConfigurationRegistry<SwipeView>> _platformConfigurationRegistry;

		public SwipeView()
		{
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<SwipeView>>(() => new PlatformConfigurationRegistry<SwipeView>(this));
		}

		public static readonly BindableProperty LeftItemsProperty = BindableProperty.Create(nameof(LeftItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator);
		public static readonly BindableProperty RightItemsProperty = BindableProperty.Create(nameof(RightItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator);
		public static readonly BindableProperty TopItemsProperty = BindableProperty.Create(nameof(TopItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator);
		public static readonly BindableProperty BottomItemsProperty = BindableProperty.Create(nameof(BottomItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator);

		public SwipeItems LeftItems
		{
			get { return (SwipeItems)GetValue(LeftItemsProperty); }
			set { SetValue(LeftItemsProperty, value); }
		}

		public SwipeItems RightItems
		{
			get { return (SwipeItems)GetValue(RightItemsProperty); }
			set { SetValue(RightItemsProperty, value); }
		}

		public SwipeItems TopItems
		{
			get { return (SwipeItems)GetValue(TopItemsProperty); }
			set { SetValue(TopItemsProperty, value); }
		}

		public SwipeItems BottomItems
		{
			get { return (SwipeItems)GetValue(BottomItemsProperty); }
			set { SetValue(BottomItemsProperty, value); }
		}

		public event EventHandler<SwipeStartedEventArgs> SwipeStarted;
		public event EventHandler<SwipeChangingEventArgs> SwipeChanging;
		public event EventHandler<SwipeEndedEventArgs> SwipeEnded;
		public event EventHandler CloseRequested;

		public void Close()
		{
			CloseRequested?.Invoke(this, EventArgs.Empty);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendSwipeStarted(SwipeStartedEventArgs args) => SwipeStarted?.Invoke(this, args);

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendSwipeChanging(SwipeChangingEventArgs args) => SwipeChanging?.Invoke(this, args);

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendSwipeEnded(SwipeEndedEventArgs args) => SwipeEnded?.Invoke(this, args);

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			object bc = BindingContext;

			if (LeftItems != null)
				SetInheritedBindingContext(LeftItems, bc);

			if (RightItems != null)
				SetInheritedBindingContext(RightItems, bc);

			if (TopItems != null)
				SetInheritedBindingContext(TopItems, bc);

			if (BottomItems != null)
				SetInheritedBindingContext(BottomItems, bc);
		}

		public abstract class BaseSwipeEventArgs : EventArgs
		{
			protected BaseSwipeEventArgs(SwipeDirection swipeDirection)
			{
				SwipeDirection = swipeDirection;
			}

			public SwipeDirection SwipeDirection { get; set; }
		}

		public class SwipeStartedEventArgs : BaseSwipeEventArgs
		{
			public SwipeStartedEventArgs(SwipeDirection swipeDirection) : base(swipeDirection)
			{

			}
		}

		public class SwipeChangingEventArgs : EventArgs
		{
			public SwipeChangingEventArgs(SwipeDirection swipeDirection, double offset)
			{
				SwipeDirection = swipeDirection;
				Offset = offset;
			}

			public SwipeDirection SwipeDirection { get; set; }
			public double Offset { get; set; }
		}

		public class SwipeEndedEventArgs : BaseSwipeEventArgs
		{
			public SwipeEndedEventArgs(SwipeDirection swipeDirection) : base(swipeDirection)
			{

			}
		}

		SwipeItems SwipeItemsDefaultValueCreator() => new SwipeItems();

		static object SwipeItemsDefaultValueCreator(BindableObject bindable)
		{
			return ((SwipeView)bindable).SwipeItemsDefaultValueCreator();
		}

		public IPlatformElementConfiguration<T, SwipeView> On<T>() where T : IConfigPlatform
		{
			return _platformConfigurationRegistry.Value.On<T>();
		}
	}
}