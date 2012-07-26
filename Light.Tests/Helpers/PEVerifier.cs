using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Light.Tests.Helpers {
    public static class PEVerifier {
        private static readonly string PEVerifyPath = Path.Combine(
            (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SDKs\Windows\v8.0A\WinSDK-NetFx40Tools", "InstallationFolder", ""),
            "PEVerify.exe"
        );

        public static void Verify(Assembly assembly) {
            Verify(assembly.Location, assembly);
        }

        public static void Verify(string path, Assembly assembly) {
            var start = new ProcessStartInfo(PEVerifyPath, "\"" + path + "\"") {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            using (var process = Process.Start(start)) {
                process.WaitForExit();

                var output = process.StandardOutput.ReadToEnd();
                if (process.ExitCode != 0)
                    throw new Exception(Environment.NewLine + string.Join(Environment.NewLine, ParseVerificationResults(output, assembly)));
            }
        }

        private static IEnumerable<string> ParseVerificationResults(string output, Assembly assembly) {
            var lines = Regex.Matches(output, @"^\[.+", RegexOptions.Multiline).Cast<Match>().Select(m => m.Value);
            return lines.Select(l => ResolveTokens(l, assembly));
        }

        private static string ResolveTokens(string peVerifyLine, Assembly assembly) {
            return Regex.Replace(peVerifyLine, @"(?<!offset )0x([\da-f]+)", match => {
                try {
                    var token = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
                    var member = assembly.GetModules()[0].ResolveMember(token);
                    return member.Name;
                }
                catch (Exception) {
                    return match.Value;
                }
            });
        }
    }
}
