//Credit goes to https://github.com/brianide/CommonGround
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Reflection;
using TShockAPI;

namespace CommonGround.Configuration
{
	/// <summary>
	/// An interface for defining JSON configuration files for TShock plugins.
	/// Implementors should be structs containing only the required FilePath
	/// override and a list of properties satisfying the following criteria:
	/// * Annotated with a default value via System.ComponentModel.DefaultValue
	/// * Possessed of a public getter and private setter
	/// </summary>
	public interface IPluginConfiguration
	{
		/// <summary>
		/// Specifies with path to the configuration file, underneath the TShock
		/// data directory.
		/// </summary>
		[JsonIgnore]
		string FilePath { get; }
	}

	/// <summary>
	/// Static class for loading configuration files.
	/// </summary>
	public static class PluginConfiguration
	{
		private readonly static JsonSerializerSettings settings = new JsonSerializerSettings()
		{
			DefaultValueHandling = DefaultValueHandling.Populate,
			ContractResolver = new DefaultValueContractResolver()
		};

		/// <summary>
		/// Returns the specified configuration interface with its default values
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T LoadDefault<T>() => JsonConvert.DeserializeObject<T>("{}", settings);

		/// <summary>
		/// Populates the specified configuration interface
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Load<T>() where T : struct, IPluginConfiguration
		{
			string filePath = default(T).FilePath;
			TShock.Log.ConsoleInfo("Loading configuration from {0}", filePath);

			T config;
			if (!File.Exists(filePath))
			{
				config = LoadDefault<T>();
			}
			else
			{
				try
				{
					config = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath), settings);
				}
				catch (JsonReaderException e)
				{
					TShock.Log.ConsoleError("Error loading {0}: {1}", filePath, e.Message);
					config = LoadDefault<T>();
				}
			}

			File.WriteAllText(filePath, JsonConvert.SerializeObject(config, Formatting.Indented));
			return config;
		}

		public static string Stringify<T>(T config) where T : IPluginConfiguration => JsonConvert.SerializeObject(config);
	}

	class DefaultValueContractResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var prop = base.CreateProperty(member, memberSerialization);
			bool tagged = member.GetCustomAttribute<System.ComponentModel.DefaultValueAttribute>() != null && (member as PropertyInfo)?.SetMethod != null;
			prop.Writable = tagged;
			prop.ShouldSerialize = _ => tagged;
			prop.ShouldDeserialize = _ => tagged;
			return prop;
		}
	}
}
