using System.Windows.Controls;
using System.Windows.Input;


namespace Business.Wpf.UX
{
    public static class CopyExtensions
    {


        public static void CopyGridContentsToClipBoard(this DataGrid grid, DataGridClipboardCopyMode CopyMode = DataGridClipboardCopyMode.IncludeHeader, bool SelectAllCells = true)
        {
            var mode = grid.SelectionMode;
            if (SelectAllCells)
            {
                if (mode == DataGridSelectionMode.Single)
                {
                    grid.SelectionMode = DataGridSelectionMode.Extended;
                }
                grid.SelectAllCells();
            }

            grid.ClipboardCopyMode = CopyMode;
            ApplicationCommands.Copy.Execute(null, grid);

            if (SelectAllCells)
            {
                grid.UnselectAllCells();
                if (mode == DataGridSelectionMode.Single)
                {
                    grid.SelectionMode = DataGridSelectionMode.Single;
                }
            }
        }


    }
}
