﻿#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using Lokad.Cqrs.Build.Engine;
using Lokad.Cqrs.Core.Dispatch;
using Lokad.Cqrs.Evil;
using Lokad.Cqrs.Feature.DefaultInterfaces;
using Lokad.Cqrs.Feature.TestPartition;
using NUnit.Framework;

namespace Lokad.Cqrs.Tests
{
	[TestFixture]
	public sealed class SmokeTests
	{
		// ReSharper disable InconsistentNaming

		#region Setup/Teardown

		static CloudEngineHost BuildHost()
		{
			var engine = new CloudEngineBuilder();
			engine.AddMessageClient("memory:test-in");
			engine.AddMemoryPartition("test-hi", "test-bye");
			engine.AddMemoryRouter("test-in", e =>
				{
					if (e.Items.Any(i => i.MappedType == typeof (Hello)))
						return "memory:test-hi";
					if (e.Items.Any(i => i.MappedType == typeof (Bye)))
						return "memory:test-bye";
					return "memory:test-what";
				});
			
			return engine.Build();
		}

		#endregion

		[DataContract]
		public sealed class Hello : IMessage
		{
			[DataMember(Order = 1)]
			public string Word { get; set; }
		}

		[DataContract]
		public sealed class Bye : IMessage
		{
			[DataMember(Order = 1)]
			public string Word { get; set; }
		}

		public sealed class DoSomething : IConsume<Hello>, IConsume<Bye>
		{
			void Record(string name, string value)
			{
				if (value.Length > 10)
				{
					Trace.WriteLine(string.Format("{0}: {1}... ({2})", name, value.Substring(0,9), value.Length));
				}
				else
				{
					Trace.WriteLine(string.Format("{0}: {1}", name, value));
				}
				
			}

			public void Consume(Bye message)
			{
				Record("Bye", message.Word);
			}

			public void Consume(Hello message)
			{
				Record("Hello", message.Word);
			}
		}

		[Test]
		public void Test()
		{
			using (var host = BuildHost())
			{
				host.Initialize();

				var client = host.Resolve<IMessageSender>();

				client.Send(new Hello {Word = "HI!"});
				client.Send(new Hello {Word = new string(')', 9000)});
				client.DelaySend(1.Seconds(), new Hello { Word = "Let's meet." });
				client.DelaySend(2.Seconds(), new Bye { Word = "Farewell..."});

				using (var cts = new CancellationTokenSource())
				{
					var task = host.Start(cts.Token);
					Thread.Sleep(10.Seconds());


					
					cts.Cancel(true);
					task.Wait(5.Seconds());
				}
				// second run
				using (var cts = new CancellationTokenSource())
				{
					var task = host.Start(cts.Token);
					Thread.Sleep(2.Seconds());
					cts.Cancel(true);
					task.Wait(5.Seconds());
				}
			}
		}

		[Test]
		public void Test2()
		{
		}
	}
}