var example = function(){
   var x = "I'm a example";

   var whoIs = function() {
     return x;
   }

   return {
      itIs: whoIs
   }
}();

example.itIs();

// I can't access x because it's private in this concept

// Revealing Module Pattern 
// Immediately Invoked Function Expression