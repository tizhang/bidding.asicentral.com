'use strict';

/* eslint-disable */
(function (global, factory) {
  if (typeof define === 'function' && define.amd) {
    define(['angular'], factory);
  } else if (typeof module !== 'undefined' && module.exports) {
    module.exports = factory();
  } else {
    global.SimpleModelFactory = factory(angular);
  }
})(this, function (angular) {

  var module = angular.module('bidding.models');

  // compression
  var forEach = angular.forEach,
    extend = angular.extend,
    copy = angular.copy;

  // keywords that are reserved for model instance
  // internal usage only and to be stripped
  // before sending to server
  var instanceKeywords = ['$$array', '$save', '$destroy',
    '$pending', '$rollback', '$diff', '$update', '$commit', '$copy'];

  // keywords that are reserved for the model static
  // these are used to determine if a attribute should be extended
  // to the model static class for like a helper that is not a http method
  var staticKeywords = ['instance', 'list', 'defaults', 'pk', 'map'];

  // Deep extends
  // http://stackoverflow.com/questions/15310935/angularjs-extend-recursive
  var extendDeep = function (dst) {
    forEach(arguments, function (obj) {
      if (obj !== dst) {
        forEach(obj, function (value, key) {
          if (instanceKeywords.indexOf(key) === -1) {
            if (dst[key]) {
              if (angular.isArray(dst[key])) {
                dst[key].concat(value.filter(function (v) {
                  var vv = dst[key].indexOf(v) !== -1;
                  if (vv) extendDeep(vv, v);
                  return vv;
                }));
              } else if (angular.isObject(dst[key])) {
                extendDeep(dst[key], value);
              } else {
                // if value is a simple type like a string, boolean or number
                // then assign it
                dst[key] = value;
              }
            } else if (!angular.isFunction(dst[key])) {
              dst[key] = value;
            }
          }
        });
      }
    });
    return dst;
  };

  // Create a shallow copy of an object and clear other fields from the destination
  // https://github.com/angular/angular.js/blob/master/src/ngResource/resource.js#L30
  var shallowClearAndCopy = function (src, dst) {
    dst = dst || {};

    // Remove any properties in destination that were not
    // returned from the source
    forEach(dst, function (value, key) {
      if (!src.hasOwnProperty(key) && key.charAt(0) !== '$') {
        delete dst[key];
      }
    });

    for (var key in src) {

      if (src.hasOwnProperty(key) && key.charAt(0) !== '$') {
        // For properties common to both source and destination,
        // check for object references and recurse as needed. Route around
        // arrays to prevent value/order inconsistencies
        if (angular.isObject(src[key]) && angular.isObject(dst[key]) && !angular.isArray(src[key])) {
          dst[key] = shallowClearAndCopy(src[key], dst[key]);
        } else {
          // Not an object, so just overwrite with value from source
          dst[key] = src[key];
        }
      }
    }

    return dst;
  };


  module.provider('$simpleModelFactory', function () {
    var provider = this;

    provider.defaultOptions = {

      /**
       * Primary key of the model
       */
      pk: 'id',

      /**
       * Default values for a new instance.
       * This will only be populated if the property
       * is undefined.
       *
       * Example:
       *      defaults: {
       *          'create': new Date()
       *      }
       */
      defaults: {},

      /**
       * Attribute mapping.  Tranposes attributes
       * from a response to a different attribute.
       *
       * Also handles 'has many' and 'has one' relations.
       *
       * Example:
       *      map: {
       *          // transpose `animalId` to
       *          // `id` on our instance
       *          'id': 'animalId',
       *
       *          // transposes `animal` attribute
       *          // to an array of `AnimalModel`'s
       *          'animal': AnimalModel.List,
       *
       *          // transposes `location` attribute
       *          // to an instance of `LocationModel`
       *          'location': LocationModel
       *      }
       */
      map: {},

      /**
       * Instance level extensions/helpers.
       *
       * Example:
       *      instance: {
       *          'name': function() {
       *              return this.first + ' ' + this.last
       *          }
       *      }
       */
      instance: {},

      /**
       * List level extensions/helpers.
       *
       * Example:
       *
       *      list: {
       *          'namesById': function(id){
       *              return this.find(function(u){ return u.id === id; });
       *          }
       *      }
       *
       */
      list: {}
    };

    provider.$get = ['$rootScope', '$log', function ($rootScope, $log) {

      /**
       * Model factory.
       *
       * Example usages:
       *       $simpleModelFactory();
       *       $simpleModelFactory({ ... });
       */
      function modelFactory(options) {

        // copy so we also extend our defaults and not override
        options = extendDeep({}, copy(provider.defaultOptions), options);

        //
        // Collection
        // ------------------------------------------------------------
        //

        /**
         * Model list instance.
         * All raw objects passed will be converted to an instance of this model.
         *
         * If we `push` a item into an existing collection, a pointer will be made
         * so on a destroy items will be removed from the array as well.
         *
         * Example usages:
         *       var zoos = new Zoo.List([ {}, ... ]);
         */
        function ModelCollection(value) {
          value = value || [];

          // wrap each obj
          value.forEach(function (v, i) {
            // this should not happen but prevent blow up
            if (v === null || v === undefined) return;

            // reset to new instance
            value[i] = wrapAsNewModelInstance(v, value);
          });

          // override push to set an instance
          // of the list on the model so destroys will chain
          var __oldPush = value.push;
          value.push = function () {
            // Array.push(..) allows to pass in multiple params
            var args = Array.prototype.slice.call(arguments);

            for (var i = 0; i < args.length; i++) {
              args[i] = wrapAsNewModelInstance(args[i], value);
            }

            __oldPush.apply(value, args);
          };

          // add list helpers
          if (options.list) {
            extend(value, options.list);
          }

          return value;
        };

        // helper function for creating a new instance of a model from
        // a raw JavaScript obj. If it is already a model, it will be left
        // as it is
        var wrapAsNewModelInstance = function (rawObj, arrayInst) {
          // create an instance
          var inst = rawObj.constructor === Model ?
            rawObj : new Model(rawObj);

          // set a pointer to the array
          inst.$$array = arrayInst;

          return inst;
        };


        //
        // Model Instance
        // ------------------------------------------------------------

        /**
         * Model instance.
         *
         * Example usages:
         *       var zoo = new Zoo({ ... });
         */
        function Model(value) {
          var instance = this,
            commits = [];

          // if the value is undefined, create a empty obj
          value = value || {};

          // build the defaults but only on new instances
          forEach(options.defaults, function (v, k) {
            // only populates when not already defined
            if (value[k] === undefined) {
              if (typeof v === 'function') {
                // pass the value so you can combine things
                // this could be tricky if you have defaults that rely on other defaults ...
                // like: { name: function(val) { return val.firstName + val.lastName }) }
                value[k] = copy(v(value));
              } else {
                value[k] = copy(v);
              }
            }
          });

          // Map all the objects to new names or relationships
          forEach(options.map, function (v, k) {
            if (v.isModelFactory) {
              value[k] = new v(value[k]);
            } else if (typeof v === 'function') {
              // if its a function, invoke it,
              // this would be helpful for seralizers
              // like: map: { date: function(val){ return moment(val) } }
              value[k] = v(value[k], value);
            } else {
              value[k] = value[v];
              delete value[v];
            }
          });

          // copy values to the instance
          extend(instance, value);

          // copy instance level helpers to this instance
          extend(instance, copy(options.instance));

          if (typeof options.init === 'function') {
            options.init(instance);
          }

          /**
           * If the item is associated with an array, it will automatically be removed.
           */
          instance.$destroy = function () {
            var arr = instance.$$array;
            if (arr) {
              arr.splice(arr.indexOf(instance), 1);
            }
          };

          /**
           * Display the difference between the original data and the
           * current instance.
           * https://github.com/flitbit/diff
           */
          instance.$diff = function () {
            return DeepDiff.deep(old, instance, function (path, key) {
              return key[0] === '$';
            });
          };

          /**
           * Commits the change the commits bucket for rollback later if needed.
           */
          instance.$commit = function () {
            // stringify it so you have a clean instance
            commits.push(angular.toJson(instance));
            return instance;
          };

          /**
           * Reverts the current instance back either the latest instance
           * or you can pass a specific instance on the commits stack.
           */
          instance.$rollback = function (version) {
            var prevCommit = commits[version || commits.length - 1];
            instance.$update(JSON.parse(prevCommit));
            return instance;
          };

          /**
           * Extends the properties of the new object onto
           * the current object without replacing it.  Helpful
           * when copying and then re-copying new props back
           */
          instance.$update = function (n) {
            shallowClearAndCopy(n, instance);
            return instance;
          };


          /**
           * Creates a copy by taking the raw data values and by
           * creating a new instance of the model.
           */
          instance.$copy = function () {
            // get the raw data of the model
            var rawData = angular.toJson(this);

            // ..then wrap it into a new instance to create a clone
            return new Model(angular.fromJson(rawData));
          };

          // Create a copy of the value last so we get all the goodies,
          // like default values and whatnot.
          instance.$commit();
        }

        //
        // Model Static
        // ------------------------------------------------------------

        /**
         * Remove instances of reserved keywords
         * before sending to server/json.
         */
        Model.$strip = function (args) {
          // todo: this needs to account for relationships too?
          // either make recursive or chain invoked
          if (args && typeof args === 'object') {
            forEach(args, function (v, k) {
              if (instanceKeywords.indexOf(k) > -1) {
                delete args[k];
              }
            });
          }
          return args;
        };

        // extend the static class with arguments that are not internal
        forEach(options, function (v, k) {
          if (staticKeywords.indexOf(k) === -1) {
            Model[k] = v;
          }
        });

        // has to be at end for depedency reasons
        ModelCollection.isModelFactory = true;
        Model.isModelFactory = true;

        Model.List = ModelCollection;

        return Model;
      }

      return modelFactory;
    }];
  });
});
/* eslint-enable */
