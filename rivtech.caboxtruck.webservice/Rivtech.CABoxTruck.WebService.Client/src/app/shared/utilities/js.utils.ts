import { Injectable } from '@angular/core';

@Injectable()
export default class JsUtils {

    public static unflatten(flat: any) {

        let arr = flat;

        var tree = [],
            mappedArr = {},
            arrElem,
            mappedElem;

        // First map the nodes of the array to an object -> create a hash table.
        for (var i = 0, len = arr.length; i < len; i++) {
            arrElem = arr[i];
            mappedArr[arrElem.id] = arrElem;
            mappedArr[arrElem.id]['children'] = [];
        }


        for (var id in mappedArr) {
            if (mappedArr.hasOwnProperty(id)) {
                mappedElem = mappedArr[id];
                // If the element is not at the root level, add it to its parent array of children.
                if (mappedElem.parentid) {
                    mappedArr[mappedElem['parentid']]['children'].push(mappedElem);
                }
                // If the element is at the root level, add it to first level elements array.
                else {
                    tree.push(mappedElem);
                }
            }
        }

        return tree;
    }

    public static mergeArrayObjects(arr1, arr2, comparator) {

        arr1 = arr1.sort(comparator);
        arr2 = arr2.sort(comparator);

        return arr1.map((item, i) => {

            if (item.id === arr2[i].id) {

                return Object.assign({}, item, arr2[i])
            }
        })
    }

    public static getNestedChildren(arr, parentId) {
        var out = []
        for (var i in arr) {
    
          if (arr[i].parentId == parentId && arr[i].parentId != arr[i].id) {
    
            var children = this.getNestedChildren(arr, arr[i].id)
    
            if (children.length) {
              arr[i].children = children;
            }
    
            out.push(arr[i]);
          }
        }
    
        return out
      }

      public static mergeByKey(a1: any[], a2: any[], key: string): any[] {
        return a1.map(itm => ({
            ...a2.find((item) => (item[key] === itm[key]) && item),
            ...itm
        }));
    }
}
