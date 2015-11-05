﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit
{
	class VsixTestCase : XunitTestCase
	{
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
		public VsixTestCase () { }

		public VsixTestCase (IMessageSink messageSink, Xunit.Sdk.TestMethodDisplay defaultMethodDisplay, ITestMethod testMethod,
			string vsVersion, string rootSuffix, bool? newIdeInstance, int timeoutSeconds, bool? recycleOnFailure, object[] testMethodArguments = null)
			: base (messageSink, defaultMethodDisplay, testMethod, testMethodArguments)
		{
			VisualStudioVersion = vsVersion;
			RootSuffix = rootSuffix;
			NewIdeInstance = newIdeInstance;
			TimeoutSeconds = timeoutSeconds;
			RecycleOnFailure = recycleOnFailure;
		}

		public string VisualStudioVersion { get; private set; }

		public string RootSuffix { get; private set; }

		public bool? NewIdeInstance { get; private set; }

		public int TimeoutSeconds { get; private set; }

		public bool? RecycleOnFailure { get; private set; }

		protected override void Initialize ()
		{
			base.Initialize ();

			DisplayName += " > vs" + VisualStudioVersion;

			// Register VS version as a trait, so that it can be used to group runs.
			Traits["VisualStudioVersion"] = new List<string> (new[] { VisualStudioVersion });
		}

		protected override string GetUniqueID ()
		{
			return base.GetUniqueID () + "-" + VisualStudioVersion;
		}

		public override void Serialize (IXunitSerializationInfo data)
		{
			base.Serialize (data);
			data.AddValue ("VisualStudioVersion", VisualStudioVersion);
			data.AddValue (SpecialNames.VsixAttribute.RootSuffix, RootSuffix);
			data.AddValue (SpecialNames.VsixAttribute.NewIdeInstance, NewIdeInstance);
			data.AddValue (SpecialNames.VsixAttribute.TimeoutSeconds, TimeoutSeconds);
		}

		/// <inheritdoc/>
		public override void Deserialize (IXunitSerializationInfo data)
		{
			base.Deserialize (data);
			VisualStudioVersion = data.GetValue<string> ("VisualStudioVersion");
			RootSuffix = data.GetValue<string> (SpecialNames.VsixAttribute.RootSuffix);
			NewIdeInstance = data.GetValue<bool?> (SpecialNames.VsixAttribute.NewIdeInstance);
			TimeoutSeconds = data.GetValue<int> (SpecialNames.VsixAttribute.TimeoutSeconds);
		}
	}
}