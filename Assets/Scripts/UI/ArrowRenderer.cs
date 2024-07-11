using UnityEngine;
using UnityEngine.UI;

namespace Chessed
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class ArrowRenderer : Graphic
    {
        public RectTransform from, to;
        public float thickness = 10f;
        public Vector2 headSize = new Vector2(20f, 20f);

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (from == null || to == null) return;

            Vector2 start = from.anchoredPosition;
            Vector2 end = to.anchoredPosition;
            Vector2 dir = (end - start).normalized;
            
            // Calculate the point where the line ends and the arrowhead starts
            Vector2 lineEnd = end - dir * headSize.y;
            
            // Calculate vertices for the line
            Vector2 normal = new Vector2(-dir.y, dir.x) * thickness * .5f;
            Vector2[] verts = { start + normal, start - normal, lineEnd - normal, lineEnd + normal };

            // Draw line
            DrawQuad(verts, vh);
            
            // Calculate vertices for arrowhead
            Vector2 headBase1 = lineEnd - normal * headSize.x * .5f;
            Vector2 headBase2 = lineEnd + normal * headSize.x * .5f;
            verts = new[] { end, headBase1, headBase2 };
            
            // Draw arrowhead
            DrawTriangle(verts, vh);
        }

        private void DrawQuad(Vector2[] verts, VertexHelper vh)
        {
            if (verts.Length != 4) return;
            
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            foreach (Vector2 vert in verts)
            {
                vertex.position = vert;
                vh.AddVert(vertex);
            }

            int startIndex = vh.currentVertCount - 4;
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        private void DrawTriangle(Vector2[] verts, VertexHelper vh)
        {
            if (verts.Length != 3) return;
            
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            foreach (Vector2 vert in verts)
            {
                vertex.position = vert;
                vh.AddVert(vertex);
            }

            int startIndex = vh.currentVertCount - 3;
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        }
    }
}