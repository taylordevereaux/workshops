using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoApp.Shared.Models;
using Windows.UI.Xaml;

namespace TodoApp.Shared.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _newTodoText;
        private State _state = State.Default;
        private int _filter = 0;

        public MainPageViewModel()
        {
            CreateNew = new SimpleCommand(ExecuteCreateNew);
            ViewAll = new SimpleCommand(() => Filter = 0);
            ViewActive = new SimpleCommand(() => Filter = 1);
            ViewInactive = new SimpleCommand(() => Filter = 2);
        }

        public State State
        {
            get => _state;
            private set
            {
                this._state = value;
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(Todos));
            }
        }

        public Visibility IsEmpty
        {
            get
            {
                if(Todos.Count() == 0)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        public int Filter
        {
            get => _filter;
            set
            {
                this._filter = value;
                OnPropertyChanged(nameof(Filter));
                OnPropertyChanged(nameof(Todos));
            }
        } // 0-all, 1-active, 2-inactives

        public IEnumerable<Todo> Todos
        {
            get
            {
                switch (Filter)
                {
                    case 0:
                        return _state.Todos;
                    case 1:
                        return _state.ActiveTodos;
                    case 2:
                        return _state.InactiveTodos;
                }

                throw new InvalidOperationException();
            }
        }

        public string NewTodoText
        {
            get => _newTodoText;
            set
            {
                this._newTodoText = value;
                OnPropertyChanged(nameof(NewTodoText));
            }
        }

        public ICommand CreateNew { get; }

        public ICommand ViewAll { get; }
        public ICommand ViewActive { get; }
        public ICommand ViewInactive { get; }

        private void ExecuteCreateNew()
        {
            var newTodo = new Todo(NewTodoText);
            State = State.WithTodos(todos => todos.Add(newTodo));
            NewTodoText = "";
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsEmpty));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangeState(Todo todo, bool isDone)
        {
            State = State.WithTodos(todos =>
            {
                var existing = todos.FirstOrDefault(t => t.KeyEquals(todo));
                Todo newTodo = existing.WithIsDone(isDone);

                return newTodo != existing ? todos.Replace(existing, newTodo) : todos;
            });
        }

        public void ChangeText(Todo todo, string newText)
        {
            State = State.WithTodos(todos =>
            {
                var existing = todos.FirstOrDefault(t => t.KeyEquals(todo));
                Todo newTodo = existing.WithText(newText);

                return todos.Replace(existing, newTodo);
            });
        }

        public void RemoveTodo(Todo todo)
        {
            State = State.WithTodos(todos =>
            {
                var existing = todos.FirstOrDefault(t => t.KeyEquals(todo));

                return todos.Remove(existing);
            });
        }
    }
}
