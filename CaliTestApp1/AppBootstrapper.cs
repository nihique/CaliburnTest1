using CaliTestApp1.Utils;
using CaliTestApp1.Views;

namespace CaliTestApp1
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Hosting;
	using System.ComponentModel.Composition.Primitives;
	using System.Linq;
	using Caliburn.Micro;

	public class AppBootstrapper : Bootstrapper<ShellViewModel>
	{
		CompositionContainer _container;

		/// <summary>
		/// By default, we are configured to use MEF
		/// </summary>
		protected override void Configure() {
		    var catalog = new AggregateCatalog(
		        AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
		        );

			_container = new CompositionContainer(catalog);

			var batch = new CompositionBatch();

			batch.AddExportedValue<IWindowManager>(new WindowManager());
			batch.AddExportedValue<IEventAggregator>(new EventAggregator());
			batch.AddExportedValue(_container);
		    batch.AddExportedValue(catalog);

			_container.Compose(batch);

            AsyncAwaitSupport.Hook();
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
		    var export = _container.GetExportedValues<object>(contract).FirstOrDefault();
			if (export != null) return export;
			throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
		}

		protected override void BuildUp(object instance)
		{
			_container.SatisfyImportsOnce(instance);
		}
	}
}