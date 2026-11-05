using System.Collections.Generic;
using ClinicApp.Interfaces;

namespace ClinicApp.Managers;

public class Repository<T> where T : IIdentifiable
{
    private List<T> _items = new List<T>();
    public int Count => _items.Count;

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T? GetById(int id)
    {
        return _items.Find(item => item.Id == id);
    }

    public List<T> GetAll()
    {
        return _items;
    }

    public bool Remove(int id)
    {
        T? item = GetById(id);

        if (item == null)
        {
            return false;
        }

        _items.Remove(item);
        return true;
    }


}