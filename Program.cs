using System;
using System.Collections.Generic;

namespace OpenClose
{
    public enum Size
    {
        Small,Medium,Large
    }
    public enum Color
    {
        Red,Green,Blue
    }
    public class Product{
        public String name;
        public Size size;
        public Color color;
       public Product(String _name,Size _size,Color _color)
       {
           name = _name;
           size = _size;
           color = _color;
       }
    }
    public class Filter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products,Size size){
            foreach(var p in products)
                if(p.size==size)
                    yield return p;

        } 
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products,Color color){
            foreach(var p in products)
                if(p.color==color)
                    yield return p;

        } 
        public IEnumerable<Product> FilterByColorAndSize(IEnumerable<Product> products,Color color,Size size){
            foreach(var p in products)
                if(p.color==color && p.size==size)
                    yield return p;

        } 
    }
    public interface ISpecification<T> {
        bool IsSatisfied(T t);
    }
    public interface IFilter<T>{
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }
    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                if(spec.IsSatisfied(item)){
                    yield return item;
                }
            }
        }
    }
    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;
        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        
        public bool IsSatisfied(Product t)
        {
            return t.color == color; 
        }
    }
    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;
        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        
        public bool IsSatisfied(Product t)
        {
            return t.size == size; 
        }
    }
    public class AndSpecification : ISpecification<Product>
    {
        private Size size;
        private Color color;
        public AndSpecification(Size size,Color color)
        {
            this.size = size;
            this.color = color;
        }
        
        public bool IsSatisfied(Product t)
        {
            return t.size == size && t.color==color; 
        }
    }
    class Program
    { 
        static void Main(string[] args)
        {
            var apple = new Product("Apple",Size.Small,Color.Green);
            var tree = new Product("Tree",Size.Large,Color.Green);
            var house = new Product("House",Size.Large,Color.Blue);
            Product[] ps = {apple,tree,house};
            Filter filter= new Filter();
            Console.WriteLine("----------------------");
            Console.WriteLine("Old Filter By Size");
            foreach (var item in filter.FilterBySize(ps,Size.Small))
            {
                 Console.WriteLine($"[OLD] --> {item.name} is Small");
            }
            Console.WriteLine("Old Filter By Color");
            foreach (var item in filter.FilterByColor(ps,Color.Green))
            {
                 Console.WriteLine($"[OLD] --> {item.name} is Green");
            }
            Console.WriteLine("Old Filter By Color & Size");
            foreach (var item in filter.FilterByColorAndSize(ps,Color.Green,Size.Large))
            {
                 Console.WriteLine($"[OLD] --> {item.name} is Green & Large");
            }
            Console.WriteLine("----------------------");
            BetterFilter betterFilter= new BetterFilter();
            Console.WriteLine("New Filter By Size");
            foreach (var item in betterFilter.Filter(ps,new SizeSpecification(Size.Small)))
            {
                 Console.WriteLine($"[NEW] --> {item.name} is Small");
            }
            Console.WriteLine("New Filter By Color");
            foreach (var item in betterFilter.Filter(ps,new ColorSpecification(Color.Green)))
            {
                 Console.WriteLine($"[NEW] --> {item.name} is Green");
            }
            Console.WriteLine("New Filter By Color & Size");
            foreach (var item in betterFilter.Filter(ps,new AndSpecification(Size.Large,Color.Green)))
            {
                 Console.WriteLine($"[NEW] --> {item.name} is Green & Large");
            }
            Console.WriteLine("----------------------");
        }
    }
}
