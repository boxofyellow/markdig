using System.Linq;
using Markdig.Renderers.Normalize;

namespace Markdig.Extensions.Tables;

/// <summary>
/// A Normalize renderer for a <see cref="Table"/> in normalized form.
/// </summary>
/// <seealso cref="NormalizeObjectRenderer{Table}" />
public class NormalizeTableRenderer : NormalizeObjectRenderer<Table>
{
    private const string PipeSeparator = "|";
    private const string HeaderSeparator = "---";
    private const string AlignmentChar = ":";
    private const string MarginSeparator = " ";

    /// <summary>
    /// Writes the object to the specified renderer.
    /// </summary>
    protected override void Write(NormalizeRenderer renderer, Table obj)
    {
        renderer.EnsureLine();

        foreach (var row in obj.OfType<TableRow>())
        {
            renderer.Write(PipeSeparator);

            foreach (var tableCell in row)
            {
                renderer.Write(MarginSeparator);

                renderer.Render(tableCell);

                renderer.Write(MarginSeparator);
                renderer.Write(PipeSeparator);
            }

            renderer.WriteLine();

            if (row.IsHeader)
            {
                bool alignmentEnabled = obj.ColumnDefinitions.Any(c => c.Alignment != TableColumnAlign.Left);

                renderer.Write(PipeSeparator);

                foreach (var column in obj.ColumnDefinitions)
                {
                    renderer.Write(MarginSeparator);
                    if (alignmentEnabled && (column.Alignment == TableColumnAlign.Left || column.Alignment == TableColumnAlign.Center))
                    {
                        renderer.Write(AlignmentChar);
                    }
                    renderer.Write(HeaderSeparator);
                    if (alignmentEnabled && (column.Alignment == TableColumnAlign.Right || column.Alignment == TableColumnAlign.Center))
                    {
                        renderer.Write(AlignmentChar);
                    }
                    renderer.Write(MarginSeparator);
                    renderer.Write(PipeSeparator);
                }

                renderer.WriteLine();
            }
        }

        renderer.FinishBlock(true);
    }
}
