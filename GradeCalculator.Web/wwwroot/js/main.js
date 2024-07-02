const app = Vue.createApp({
    data() {
        return {
            studentName: "null",
            yearLevel: "null",
            program: "null",
            term: "null",
            gradeList: [],
            gwa: 0.00
        }
    },
    methods: {
        setStudentName: function(newName){
            this.studentName = newName
        },
        setYearLevel: function(newYearLevel){
            this.yearLevel = newYearLevel
        },
        setProgram: function(newProgram){
            this.program = newProgram
        },
        setTerm: function(newTerm){
            this.term = newTerm
        },
        pushGrade: function(addGrade){
            this.gradeList.push(addGrade)
        },
        setGWA: function(newGWA){
            this.gwa = newGWA
        }
    }
})