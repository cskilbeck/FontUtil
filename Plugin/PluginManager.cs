using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FontUtil
{
	public static class PluginManager
	{
		static List<Plugin> plugins = new List<Plugin>();

		public static void ScanDirectory(string name)
		{
            try
            {
                foreach (string file in Directory.GetFiles(name, "*.dll"))
                {
                    AddDLL(file);
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Can't find plugins directory in:\r\n\r\n" + name + "\r\n\r\nIt should be in the same folder as the executable. No plugins will be loaded.", "Plugins folder not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
		}

		public static void AppendPluginsToContextMenu(ContextMenu m, EventHandler handler)
		{
			foreach (Plugin plugin in plugins)
			{
				MenuItem o = m.MenuItems.Add(plugin.Name);
				foreach (Function function in plugin.functions)
				{
					o.MenuItems.Add(function.name, handler).Tag = function;
				}
			}
		}

		public static Function FindFunction(string pluginName, string functionName)
		{
			Plugin p = plugins.Single(pl => pl.Name == pluginName);
			return p != null ? p.FindFunction(functionName) : null;
		}

		public static void AddDLL(string name)
		{
			Plugin plugin;

			Assembly ass = Assembly.LoadFrom(name);

			string msg = "";

			foreach (Type type in ass.GetTypes())
			{
				plugin = null;

				if (type.IsClass)
				{
					foreach (MethodInfo methodInfo in type.GetMethods())
					{
						Type defaultsType = null;

						object[] attributes = methodInfo.GetCustomAttributes(typeof(PluginAttribute), true);
						object[] defaults = methodInfo.GetCustomAttributes(typeof(PluginDefaults), true);

						if(defaults.Length > 0)
						{
							PluginDefaults pidf = defaults[0] as PluginDefaults;
							if (pidf != null)
							{
								defaultsType = pidf.Type;

								if (defaultsType != null && !defaultsType.IsClass)
								{
									defaultsType = null;
								}
							}
						}

						if (attributes.Length > 0)
						{
							Type t = methodInfo.ReturnType;

							// generalize this you idiot (somehow)

							if (t == typeof(Graphic))
							{
							}
							else if (t == typeof(Matrix))
							{
							}

							ParameterInfo[] prms = methodInfo.GetParameters();
							if(prms.Length > 0)
							{
								Function function = new Function();
								function.name = methodInfo.Name;
								function.method = methodInfo;
								function.description = (attributes[0] as PluginAttribute).Description;
								function.parameters = new List<Parameter>();

								if (plugin == null)
								{
									plugin = new Plugin(type.Name);
									plugins.Add(plugin);
								}

								plugin.AddFunction(function);

								for (int i = 0; i < prms.Length; ++i)
								{
									ParameterInfo p = prms[i];
									if (!p.IsRetval)
									{
										object o = p.DefaultValue;
										if (o is System.DBNull)
										{
											o = null;
										}

										// try to find a method of defaultsType which has the same name as the parameter
										// if it exists, call it and set the value to the return value
										if (defaultsType != null)
										{
											PropertyInfo prop = defaultsType.GetProperty(p.Name, BindingFlags.Public |
																									BindingFlags.NonPublic |
																									BindingFlags.GetProperty |
																									BindingFlags.Instance |
																									BindingFlags.Static
																									);
											if (prop != null)
											{
												if (prop.CanRead)
												{
													if (prop.PropertyType.Equals(p.ParameterType))
													{
														object inst = null;
														MethodInfo m = prop.GetGetMethod(true);
														if (!m.IsStatic)
														{
															// might need to construct a dummy instance if the get-default method is not static
															ConstructorInfo c = defaultsType.GetConstructor(System.Type.EmptyTypes);
															if (c != null)
															{
																inst = c.Invoke(null);
															}
														}
														// get the default value
														o = m.Invoke(inst, null);
													}
													else
													{
														MessageBox.Show(string.Format("Property '{0}.{1}' is of type '{2}', should be '{3}'", defaultsType.Name, prop.Name, prop.PropertyType.Name, p.ParameterType.Name));
													}
												}
												else
												{
													MessageBox.Show(string.Format("Property '{0}.{1}' needs to supply a 'get' body", defaultsType.Name, prop.Name));
												}
											}
											else
											{
												// Can't find default method, no biggie
											}
										}

										Parameter param = Parameter.CreateParameter(p.Name, p.ParameterType, i, o);
										if (param != null)
										{
											function.parameters.Add(param);
											if (param.IsGraphInput)
											{
												++function.graphInputs;
											}
										}
										else
										{
											msg = msg + "\nParameter " + p.Name + " is not a recognized type";
										}
									}
									else
									{
										msg = msg + "\nParameter " + p.Name + " is optional, retval or not Input";
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
