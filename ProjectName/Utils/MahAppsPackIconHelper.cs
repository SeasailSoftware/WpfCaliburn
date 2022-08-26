using Caliburn.Micro;
using MahApps.Metro.IconPacks;
using System.Windows.Media;

namespace ProjectName.Utils
{
    public static class MahAppsPackIconHelper
    {

        public static ImageSource Icon
        {
            get
            {
                return CreateImageSource(MahApps.Metro.IconPacks.PackIconMaterialKind.AlphaHCircle, Brushes.White);
            }
        }



        public static string GetPathData(object iconKind)
        {
            string value = null;
            if (iconKind is PackIconMaterialKind)
            {
                PackIconMaterialKind key = (PackIconMaterialKind)iconKind;
                PackIconMaterialDataFactory.DataIndex.Value?.TryGetValue(key, out value);
            }
            return value;
        }


        public static DrawingGroup GetDrawingGroup(object iconKind, Brush foregroundBrush, string path)
        {
            GeometryDrawing value = new GeometryDrawing
            {
                Geometry = Geometry.Parse(path),
                Brush = foregroundBrush
            };
            return new DrawingGroup
            {
                Children =
                {
                    (System.Windows.Media.Drawing)value
                },
                Transform = GetTransformGroup(iconKind)
            };
        }

        //
        // 摘要:
        //     Gets the ImageSource for the given kind.
        public static ImageSource CreateImageSource(object iconKind, Brush foregroundBrush)
        {
            string pathData = GetPathData(iconKind);
            if (string.IsNullOrEmpty(pathData))
            {
                return null;
            }

            DrawingImage drawingImage = new DrawingImage(GetDrawingGroup(iconKind, foregroundBrush, pathData));
            drawingImage.Freeze();
            return drawingImage;
        }

        public static Transform GetTransformGroup(object iconKind)
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(GetScaleTransform(iconKind));
            transformGroup.Children.Add(new ScaleTransform(1, 1));
            transformGroup.Children.Add(new RotateTransform());
            return transformGroup;
        }

        public static ScaleTransform GetScaleTransform(object iconKind)
        {
            return new ScaleTransform(1.0, 1.0);
        }
    }
}
