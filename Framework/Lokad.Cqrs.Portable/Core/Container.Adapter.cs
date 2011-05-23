
namespace Funq
{
  public partial class Container 
  {
	  public IContainerAdapter Adapter { get; set; }
  }

  /// <summary>
  /// Allow delegation of dependencies to other IOC's
  /// </summary>
  public interface IContainerAdapter
  {
      /// <summary>
      /// Resolve Property Dependency
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      T TryResolve<T>();

      /// <summary>
      /// Resolve Constructor Dependency
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      T Resolve<T>();
  }

}