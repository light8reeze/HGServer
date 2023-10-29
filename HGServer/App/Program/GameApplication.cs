using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HGServer.App
{
    internal class GameApplication : IGameApplication, IDisposable
    {
        static private GameApplication _instance = null;

        #region data field

        private List<IComponent>    _components = null;
        private List<INetworkService>      _services   = null;

        // component, service리스트를 읽기 전용 컬랙션으로 외부에 전달하기 위해 캐싱
        private ReadOnlyCollection<IComponent>  _readOnlyComponents = null;
        private ReadOnlyCollection<INetworkService>    _readOnlyServices = null;
        private bool disposedValue;

        public ReadOnlyCollection<IComponent> Components => _readOnlyComponents;
        public ReadOnlyCollection<INetworkService> Services => _readOnlyServices;

        #endregion data field

        public GameApplication(List<IComponent> componentList, List<INetworkService> serviceList)
        {
            _components = componentList;
            _services = serviceList;

            _readOnlyComponents = _components.AsReadOnly();
            _readOnlyServices   = _services.AsReadOnly();

            Debug.Assert(_instance is null);
            _instance = this;
        }

        #region static method

        public static IReadOnlyCollection<T> GetComponentList<T>() where T : IComponent
        {
            return _instance?.GetComponent<T>();
        }

        public static IReadOnlyCollection<T> GetServiceList<T>() where T : INetworkService
        {
            return _instance?.GetService<T>();
        }
        #endregion static method

        protected bool Initialize()
        {
            bool result = false;
            foreach(var service in _services) 
            {
                if (service.OnAppInitialize() is false)
                    return false;
            }

            return result;
        }
        protected bool LazyInitialize()
        {
            throw new NotImplementedException();
        }

        protected bool Run()
        {
            foreach (var service in _services)
            {
                if (service.StartService() is false)
                    return false;
            }

            return true;
        }

        protected void Close()
        {
            foreach (var service in _services)
            {
                service.CloseService();
            }
        }

        public bool Start()
        {
            bool result;
            result = Initialize();
            if (result is false)
                return false;

            result = LazyInitialize();
            if (result is false)
                return false;

            result = Run();
            if(result is false) 
                return false;

            Close();

            return true;
        }

        public IReadOnlyCollection<T> GetComponent<T>() where T : IComponent
        {
            List<T> list = new List<T>();
            foreach (var component in _components) 
            { 
                if(component is T)
                    list.Add((T)component);
            }

            return list.AsReadOnly();
        }

        public IReadOnlyCollection<T> GetService<T>() where T : INetworkService
        {
            List<T> list = new List<T>();
            foreach (var service in _services)
             {
                if(service is T)
                    list.Add((T)service);
            }

            return list.AsReadOnly();
        }

        #region Disposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        ~GameApplication()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        
        #endregion Disposable
    }
}
