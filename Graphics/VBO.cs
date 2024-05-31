using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;


namespace Agario.Graphics
{
    internal class VBO
    {
        // Идентификатор буфера
        public int ID;
        // Конструктор, принимающий список индексов
        public VBO(List<Vector3> data)
        {
            // Генерация буфера и сохранение его ID
            ID = GL.GenBuffer();
            // Привязка буфера как элементного массива
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            // Передача данных в буфер
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector3.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }
        public VBO(List<Vector2> data)
        {
            ID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector2.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }
        // Метод для привязки буфера
        public void Bind() { GL.BindBuffer(BufferTarget.ArrayBuffer, ID); }
        // Метод для отвязки буфера
        public void Unbind() { GL.BindBuffer(BufferTarget.ArrayBuffer, 0); }
        // Метод для удаления буфера
        public void Delete() { GL.DeleteBuffer(ID); }
    }
}