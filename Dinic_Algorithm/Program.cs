using System;
using System.Collections.Generic;
using System.Linq;

namespace Dinic_Algorithm
{
    internal class Program
    {
        class Arco
        {
            int t, rev, capacity, _flow;
            //Класс дуги - используется для создания каждого ребра графа
            public Arco(int t, int rev, int capacity)
            {
                this.t = t;
                this.rev = rev;
                this.capacity = capacity;
            }
            //Создаём граф, создав расположение с вершинами дуги
            public static List<Arco>[] CreateGraph(int nodes)
            {
                //Количество вершин в графе
                var graph = new List<Arco>[nodes];
                for (int i = 0; i < nodes; i++)
                    graph[i] = new List<Arco>();
                //Список возвращает граф
                return graph;
            }

            public static void AddBorder(List<Arco>[] graph, int vertice, int dest, int capacity)
            {
                graph[vertice].Add(new Arco(dest, graph[dest].Count(), capacity));
                graph[dest].Add(new Arco(vertice, graph[vertice].Count() - 1, 0));
            }

            static bool BFS(List<Arco>[] graph, int root, int dest, int[] dist)
            {
                Fill(dist, -1);// Заполнение массива с -1
                dist[root] = 0;
                var queue = new int[graph.Count()]; // создается очередь для добавления оставшихся значений
                var sizeQ = 0;
                queue[sizeQ++] = root;
                for (int i = 0; i < sizeQ; i++)
                {
                    var u = queue[i];
                    foreach (Arco aux in graph[u])
                    {
                        if (dist[aux.t] < 0 && aux._flow < aux.capacity)
                        {
                            dist[aux.t] = dist[u] + 1;
                            queue[sizeQ++] = aux.t;
                        }
                    }
                }
                return dist[dest] >= 0;
            }

            static int Dfs(List<Arco>[] Graph, int[] ptr, int[] dist, int dest, int u, int root)
            {
                if (u == dest)
                    return root;
                for (; ptr[u] < Graph[u].Count(); ++ptr[u])
                {
                    var e = Graph[u][(ptr[u])];
                    if (dist[e.t] == dist[u] + 1 && e._flow < e.capacity)
                    {
                        var df = Dfs(Graph, ptr, dist, dest, e.t, Math.Min(root, e.capacity - e._flow));
                        if (df > 0)
                        {
                            e._flow += df;
                            Graph[e.t][e.rev]._flow -= df;
                            return df;
                        }
                    }
                }
                return 0;
            }

            public static int MaxFlow(List<Arco>[] graph, int root, int dest)
            {
                var flow = 0;
                int[] dist = new int[graph.Length];
                while (BFS(graph, root, dest, dist))
                {
                    var ptr = new int[graph.Length];
                    while (true)
                    {
                        var df = Dfs(graph, ptr, dist, dest, root, 50000);
                        if (df == 0)
                            break;
                        flow += df;
                    }
                }
                return flow;
            }


            private static void Generator(int nodes)
            {
                var graph = CreateGraph(nodes);
                var rnd1 = new Random();
                for (int i = 0; i < rnd1.Next(1000); i++)
                {
                    AddBorder(graph, i, rnd1.Next(1000), 1000);
                }
                Console.WriteLine("Максимальный поток: " + MaxFlow(graph, 0, 10));
            }

            public static void Main(String[] args)
            {
                Generator(50);
            }
            public static void Fill(IList<int> array, int value)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    array[i] = value;
                }
            }
        }

    }
}