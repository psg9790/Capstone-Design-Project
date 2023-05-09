using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Excluded
{
    public class IconUpdater : MonoBehaviour
    {
        public string IconFolder = @"C:\GitHub\HE4DTEST\Assets\FantasyIconsPackFree";
        public string IconFolderRef = @"C:\GitHub\HeroEditor\Assets\HeroEditor";

        public void Start()
        {
            var files = Directory.GetFiles(IconFolder, "*.png", SearchOption.AllDirectories);
            var filesRef = Directory.GetFiles(IconFolderRef, "*.png", SearchOption.AllDirectories).Where(i => i.Contains(@"\Icons\") && !i.Contains(@"\Undead\")).ToList();

            foreach (var file in files)
            {
                var fname = Path.GetFileNameWithoutExtension(file).Split('(')[0].Trim();
                var found = filesRef.Where(i => Path.GetFileNameWithoutExtension(i) == fname).ToList();

                if (found.Count > 1)
                {
                    var folder = Path.GetFileName(Path.GetDirectoryName(file));

                    found = found.Where(i => i.Contains($@"\{folder}\")).ToList();
                }

                if (found.Count == 0)
                {
                    Debug.LogWarning($"Not found: {fname}");
                }
                else if (found.Count == 1)
                {
                    Debug.Log($"Found: {fname}");

                    File.WriteAllBytes(file, File.ReadAllBytes(found[0]));
                    File.Move(file, $"{Path.Combine(Path.GetDirectoryName(file), fname + ".png")}");
                }
                else
                {
                    Debug.LogWarning($"Multiple found: {fname}");
                }
            }
        }
    }
}