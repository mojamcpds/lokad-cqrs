#region Copyright (c) 2009-2010 LOKAD SAS. All rights reserved.

// Copyright (c) 2009-2010 LOKAD SAS. All rights reserved.
// You must not remove this notice, or any other, from this software.
// This document is the property of LOKAD SAS and must not be disclosed.

#endregion

using System;
using System.IO;
using System.Threading;
using jabber.client;
using Lokad;
using Lokad.Cqrs;
using Lokad.Quality;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Newtonsoft.Json;

namespace Sample_04.Worker
{
	[UsedImplicitly]
	public class WorkerRole : CloudEngineRole
	{
		protected override ICloudEngineHost BuildHost()
		{
			// for more detail about this sample see:
			// http://code.google.com/p/lokad-cqrs/wiki/GuidanceSeries

			var jc = GetJc();


			//logger.NetworkHost = "talk.google.com";

			var host = new CloudEngineBuilder()
				// this tells the server about the domain
				.Domain(d =>
					{
						// let's use Protocol Buffers!
						d.UseProtocolBuffers();
						d.InCurrentAssembly();
						d.WithDefaultInterfaces();
					})
				// we'll handle all messages incoming to this queue
				.HandleMessages(mc =>
					{
						mc.ListenTo("sample-04");
						mc.WithSingleConsumer();

						// let's record failures to the specified blob container
						//mc.LogExceptionsToBlob("sample-04-errors", RenderAdditionalContent);
						mc.WhenMessageHandlerFails((message, exception) => jc.Message("rinat@abdullin.com", ":-( " + exception.Message));
					})
				// when we send message - default it to this queue as well
				.SendMessages(m => m.DefaultToQueue("sample-04"))
				.Build();


			return host;
		}

		JabberClient GetJc()
		{
			var jc = new JabberClient
				{
					User = "sample",
					Server = "abdullin.com",
					NetworkHost = "talk.l.google.com",
					Password = "yh3VL2CXEIw71vk44ZrU",
					AutoLogin = true,
					AutoPresence = true,
					AutoReconnect = 60,
					Resource = string.Format(
					"{0}: {1}", RoleEnvironment.CurrentRoleInstance.Role.Name,
					RoleEnvironment.CurrentRoleInstance.Id)
				};


			var auth = new ManualResetEvent(false);

			jc.OnInvalidCertificate += (o, certificate, chain, errors) => true;
			jc.OnAuthenticate += sender => auth.Set();
			jc.Connect();
			

			if (!auth.WaitOne(6000))
			{
				throw new InvalidOperationException("Failed to authenticate");
			}
			jc.Message("rinat@abdullin.com", "Started");
			return jc;
		}

		static void RenderAdditionalContent(UnpackedMessage message, Exception exception, TextWriter builder)
		{
			
			builder.WriteLine("Content");
			builder.WriteLine("=======");
			builder.WriteLine(message.ContractType.Name);
			try
			{
				// we'll use JSON serializer for printing messages nicely
				builder.WriteLine(JsonConvert.SerializeObject(message.Content, Formatting.Indented));
			}
			catch (Exception ex)
			{
				builder.WriteLine(ex.ToString());
			}
		}

		public override bool OnStart()
		{
			DiagnosticMonitor.Start("DiagnosticsConnectionString");
			// we send first ping message, when host starts
			WhenEngineStarts += SendFirstMessage;

			return base.OnStart();
		}

		static void SendFirstMessage(ICloudEngineHost host)
		{
			var sender = host.Resolve<IMessageClient>();
			var game = Rand.String.NextWord();
			sender.Send(new PingPongCommand(0, game));
		}
	}
}