
import { Pandoc, PandocOutFormat, PandocData } from "pandoc-ts";



export class program{
    main():void{
        
        const outs: PandocOutFormat[] = [
            { name: "markdown", format: "gfm", outBin: false },
            { name: "markdownFile", format: "gfm", fname: "sample.md" },
            { name: "html", format: "html", fname: "sample.html" },
          ];
          
          const pandocInstance = new Pandoc("docx", outs);
          
          pandocInstance.convertFile("test pandoc.docx", (result, err) => {
            if (err) {
              console.error(err);
            }
            if (result) {
              console.log(result.markdown);
              console.log(result.markdownFile);
              console.log(result.html);
            }
          });
    }

    mainAsync():Promise<void>{
        return new Promise<void>((Resolve, Reject)=>{
            try{
                this.main();
                Resolve();
            }
            catch(e){
                Reject(e);
            }
        });
    }
}



var p = new program();
p.main();

